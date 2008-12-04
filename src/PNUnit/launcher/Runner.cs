using System;
using System.Collections;
using System.Threading;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;

using log4net;

using NUnit.Core;

using PNUnit.Framework;

namespace PNUnit.Launcher
{
    public class Runner: MarshalByRefObject, IPNUnitServices
    {

        private static readonly ILog log = LogManager.GetLogger("launcher");
        private const string agentkey = "_AGENT";

        private ParallelTest mTestGroup;
        private Thread mThread = null;
        private IList mResults = null;
        private Object mResultLock = new Object();
        private ManualResetEvent mFinish;
        private Hashtable mBarriers;
        private int mLaunchedTests;
        private Hashtable mBarriersOfTests;

        public Runner(ParallelTest test)
        {
            mTestGroup = test;
            mResults = new ArrayList();
        }

        public string TestGroupName
        {
            get{ return mTestGroup.Name; }
        }

        public void Run()
        {
            if( mTestGroup.Tests.Length == 0 )
            {
                log.Fatal("No tests to run, exiting");
                return;
            }
            mThread = new Thread(new ThreadStart(ThreadProc));
            mThread.Start();
        }

        public void Join()
        {
            if( mThread != null )
                mThread.Join();
        }

        private void ThreadProc()
        {
            log.DebugFormat("Thread created for TestGroup {0} with {1} tests", mTestGroup.Name, mTestGroup.Tests.Length);
            mFinish = new ManualResetEvent(false);
            mBarriers = new Hashtable();
            mBarriersOfTests = new Hashtable();
            init = false;
            RemotingServices.Marshal(this, mTestGroup.Name);

            mLaunchedTests = 0;
            foreach( TestConf test in mTestGroup.Tests )
            {
                if (test.Machine.StartsWith(agentkey))
                    test.Machine = mTestGroup.Agents[int.Parse(test.Machine.Substring(agentkey.Length))];

                Launcher.Log(string.Format("Starting {0} test {1} on {2}", mTestGroup.Name, test.Name, test.Machine));
                // contact the machine
                try
                {
                    IPNUnitAgent agent = (IPNUnitAgent)
                        Activator.GetObject(
                        typeof(IPNUnitAgent), 
                        string.Format(
                        "tcp://{0}/{1}", 
                        test.Machine, 
                        PNUnit.Framework.Names.PNUnitAgentServiceName));

                    lock( mResultLock )
                    {
                        ++mLaunchedTests;
                    }

                    agent.RunTest(new PNUnitTestInfo(test.Name, test.Assembly, test.TestToRun, test.TestParams, this, test.StartBarrier, test.EndBarrier, test.WaitBarriers));
                }
                catch( Exception e )
                {
                    Launcher.LogError(
                        string.Format("An error occurred trying to contact {0} [{1}]", 
                            test.Machine, e.Message));

                    lock( mResultLock )
                    {
                        --mLaunchedTests;
                    }
                }
            }
            
            log.DebugFormat("Thread going to wait for results for TestGroup {0}", mTestGroup.Name);
            if( HasToWait() )
                // wait for all tests to end
                mFinish.WaitOne();

            log.DebugFormat("Thread going to wait for NotifyResult to finish for TestGroup {0}", mTestGroup.Name);
            Thread.Sleep(500); // wait for the NotifyResult call to finish
            RemotingServices.Disconnect(this);
            log.DebugFormat("Thread going to finish for TestGroup {0}", mTestGroup.Name);
        }
        
        private bool HasToWait()
        {
            lock( mResultLock )
            {
                return (mLaunchedTests > 0) && (mResults.Count < mLaunchedTests);
            }
        }

        public TestResult[] GetTestResults()
        {
            lock(mResultLock)
            {
                TestResult[] result = new TestResult[mResults.Count];
                int i = 0;
                foreach( TestResult res in mResults )
                    result[i++] = res;

                return result;
            }
        }

        #region MarshallByRefObject
        // Lives forever
        public override object InitializeLifetimeService()
        {
            return null;
        }
        #endregion

        #region IPNUnitServices

        public void NotifyResult(string TestName, TestResult result)
        {
            log.DebugFormat("NotifyResult called for TestGroup {0}, Test {1}",
                mTestGroup.Name, TestName);
            lock(mResultLock)
            {
                log.DebugFormat("NotifyResult lock entered for TestGroup {0}, Test {1}",
                    mTestGroup.Name, TestName);

                mResults.Add(result);
                if( mResults.Count == mLaunchedTests )
                {
                    log.DebugFormat("All the tests notified the results, waking up. mResults.Count == {0}",
                        mResults.Count);
                    mFinish.Set();
                }
            }   
            lock( mBarriers )
            {
                if( mBarriersOfTests.Contains(TestName) )
                {
                    log.DebugFormat("Going to abandon barriers of test {0}", TestName);
                    IList list = (IList) mBarriersOfTests[TestName];
                    foreach( string barrier in list )
                    {
                        log.DebugFormat("Abandoning barrier {0}", barrier);
                        ((Barrier)mBarriers[barrier]).Abandon();
                    }
                }
            }

            log.DebugFormat("NotifyResult finishing for TestGroup {0}, Test {1}.",
                mTestGroup.Name, TestName); 

            string message = string.Format("Result for TestGroup {0}, Test {1}: {2}",
                mTestGroup.Name, TestName, result.IsSuccess ? "PASS" : "FAIL");

            if( result.IsSuccess )
                Launcher.Log(message);
            else
                Launcher.LogError(message);
        }

        private void InitBarrier(string barrier, int Max)
        {
            lock( mBarriers )
            {
                if( ! mBarriers.Contains(barrier) )
                {
                    mBarriers.Add(barrier, new Barrier(Max));
                }
            }
        }

        private void InitTestBarriers (string testName, string barrier)
        {
            if( mBarriersOfTests.Contains(testName) )
            {
                IList listofbarriers = (IList) mBarriersOfTests[testName];
                listofbarriers.Add(barrier);
                log.DebugFormat("Adding barrier {0} to {1}", barrier, testName);
            }
            else
            {
                ArrayList list = new ArrayList();
                list.Add(barrier);
                log.DebugFormat("Adding barrier {0} to {1}", barrier, testName);
                mBarriersOfTests.Add(testName, list);
            }
        }

        private void InitTestBarriers (string testName, string[] barriers)
        {
            if (barriers != null && barriers.Length > 0)
            {
                foreach (string barrier in barriers)
                    InitTestBarriers (testName, barrier);
            }
        }

        private object sync = new object();
        private bool init = false;
        public void InitBarriers ()
        {
            lock (sync)
            {
                if (init)
                    return;

                Hashtable barriers = new Hashtable();
                for (int i=0; i< mTestGroup.Tests.Length; i++)
                {
                    AddBarrier(mTestGroup.Tests[i].StartBarrier, barriers);
                    AddBarrier(mTestGroup.Tests[i].EndBarrier, barriers);
                    AddBarrier(mTestGroup.Tests[i].WaitBarriers, barriers);

                    InitTestBarriers (mTestGroup.Tests[i].Name, mTestGroup.Tests[i].StartBarrier);
                    InitTestBarriers (mTestGroup.Tests[i].Name, mTestGroup.Tests[i].EndBarrier);
                    InitTestBarriers (mTestGroup.Tests[i].Name, mTestGroup.Tests[i].WaitBarriers);
                }

                foreach (string key in barriers.Keys)
                {
                    InitBarrier (key, (int)barriers[key]);
                }

                init = true;
            }
        }

        private void AddBarrier (string barrier, Hashtable barriers)
        {
            if (barrier != null && barrier.Trim() != string.Empty)
            {
                if(barriers.Contains(barrier))
                    barriers[barrier] = (int)barriers[barrier]+1;
                else
                    barriers[barrier] = 1;
            }
        }

        private void AddBarrier (string[] barrier, Hashtable barriers)
        {
            if (barrier != null && barrier.Length > 0)
            {
                foreach (string b in barrier)
                    AddBarrier (b, barriers);
            }
        }


        public void EnterBarrier(string barrier)
        {
             
            log.DebugFormat("Entering Barrier {0}", barrier);
            ((Barrier)mBarriers[barrier]).Enter();

        }

        #endregion
    }
}
