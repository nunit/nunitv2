using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Resources;


using PNUnit.Framework;

using NUnit.Core;
using NUnit.Util;

using log4net;

namespace PNUnit.Agent
{
    public class PNUnitTestRunner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PNUnitTestRunner));
        private PNUnitTestInfo mPNUnitTestInfo;
        private AgentConfig mConfig;
        private static object obj = new object();

        public PNUnitTestRunner(PNUnitTestInfo info, AgentConfig config)
        {
            mConfig = config;
            mPNUnitTestInfo = info;
        }

        public void Run()
        {
            log.Info("Spawning a new thread");
            Thread thread = new Thread(new ThreadStart(ThreadProc));
            thread.Start();
        }

        private void ThreadProc()
        {
            TestResult result = null;
            TestDomain testDomain = new TestDomain();

            TestConsoleAccess consoleAccess = new TestConsoleAccess();

            try
            {
                log.InfoFormat("Thread entered for Test {0}:{1} Assembly {2}",
                    mPNUnitTestInfo.TestName, mPNUnitTestInfo.TestToRun, mPNUnitTestInfo.AssemblyName);

                ConsoleWriter outStream = new ConsoleWriter(Console.Out);
                ConsoleWriter errorStream = new ConsoleWriter(Console.Error);

                bool testLoaded = MakeTest(testDomain, Path.Combine(mConfig.PathToAssemblies, mPNUnitTestInfo.AssemblyName), GetShadowCopyCacheConfig());

                if (!testLoaded)
                {
                    log.InfoFormat("Unable to locate test {0}", mPNUnitTestInfo.TestName);
                    result = BuildError("Unable to locate tests", consoleAccess);

                    mPNUnitTestInfo.Services.NotifyResult(
                        mPNUnitTestInfo.TestName, result);

                    return;
                }

                Directory.SetCurrentDirectory(mConfig.PathToAssemblies); // test directory ?
                EventListener collector = new EventCollector( outStream );

                string savedDirectory = Environment.CurrentDirectory;

                log.Info("Creating PNUnitServices in the AppDomain of the test");

                object[] param = { mPNUnitTestInfo, (ITestConsoleAccess) consoleAccess };

                try
                {
                    System.Runtime.Remoting.ObjectHandle obj
#if NET_2_0
                        = Activator.CreateInstance(
                            testDomain.AppDomain,
#else

                        = testDomain.AppDomain.CreateInstance(
#endif
                        typeof(PNUnitServices).Assembly.FullName,
                        typeof(PNUnitServices).FullName,
                        false, BindingFlags.Default, null, param, null, null, null);
                    obj.Unwrap();
                }
                catch( Exception e )
                {
                    result = BuildError(e, consoleAccess);
                    log.ErrorFormat("Error running test {0}", e.Message);
                    return;
                }

                log.Info("Running tests");

                try
                {
                    ITestFilter filter = new NUnit.Core.Filters.SimpleNameFilter(mPNUnitTestInfo.TestToRun);
                    result =
                        FindResult(
                            mPNUnitTestInfo.TestToRun,
                            testDomain.Run(collector, filter) );
                    filter = null;
                }
                catch( Exception e )
                {
                    result = BuildError(e, consoleAccess);
                    log.ErrorFormat("Error running test {0}", e.Message);
                }

            }
            finally
            {
                log.Info("Notifying the results");

                mPNUnitTestInfo.Services.NotifyResult(
                    mPNUnitTestInfo.TestName, BuildResult(result, consoleAccess));

                result = null;

                //Bug with framework
                if( IsWindows() )
                {
#if !NET_2_0
                    lock(obj)
#endif
                    {
                        testDomain.Unload();
                    }
                }
            }
        }

        private TestResult BuildResult(TestResult result, TestConsoleAccess consoleAccess)
        {
            if (!result.IsSuccess)
            {
                return new PNUnitTestResult(result, consoleAccess.GetTestOutput());
            }
            else
                return result;
        }

        private TestResult BuildError(Exception e, TestConsoleAccess consoleAccess)
        {
            TestName testName = new TestName();
            testName.Name = mPNUnitTestInfo.TestName;
            testName.FullName = mPNUnitTestInfo.TestName;
            testName.TestID = new TestID();

            TestResult result = new PNUnitTestResult(testName, consoleAccess.GetTestOutput());
            result.Error(e);
            return result;
        }

        private TestResult BuildError(string message, TestConsoleAccess consoleAccess)
        {
            TestName testName = new TestName();
            testName.Name = mPNUnitTestInfo.TestName;
            testName.FullName = mPNUnitTestInfo.TestName;
            testName.TestID = new TestID();

            TestResult result = new PNUnitTestResult(testName, consoleAccess.GetTestOutput());
            result.Failure(message, string.Empty);
            return result;
        }


        private static TestResult FindResult(string name, TestResult result)
        {
            if (result.Test.TestName.FullName == name)
                return result;

            if ( result.HasResults )
            {
                foreach( TestResult r in result.Results )
                {
                    TestResult myResult = FindResult(name, r);
                    if ( myResult != null )
                        return myResult;
                }
            }

            return null;
        }

        private bool MakeTest(TestRunner runner, string assemblyName, bool bShadowCopyCache)
        {
            TestPackage package = new TestPackage(assemblyName);
            package.Settings.Add( "ShadowCopyFiles", bShadowCopyCache );
            return runner.Load(package);
        }

        private bool GetShadowCopyCacheConfig()
        {
            if (mConfig.ShadowCopyCache != null)
                if (mConfig.ShadowCopyCache.ToLower().IndexOf("false") != -1 )
                    return false;

            return true;
        }

        #region Nested Class to Handle Events

        [Serializable]
        private class EventCollector : EventListener
        {
            private int testRunCount;
            private int testIgnoreCount;
            private int failureCount;
            private int level;

            private ConsoleWriter writer;

            StringCollection messages = new StringCollection();

            private bool debugger = false;
            private string currentTestName;

            public EventCollector( ConsoleWriter writer )
            {
                debugger = Debugger.IsAttached;
                level = 0;
                this.writer = writer;
                this.currentTestName = string.Empty;
            }

            public void RunStarted(Test[] tests)
            {
            }

            public void RunStarted(string a, int b)
            {
            }

            public void RunFinished(TestResult[] results)
            {
            }

            public void RunFinished(Exception exception)
            {
            }

            public void RunFinished(TestResult result)
            {
            }

            public void TestFinished(TestResult testResult)
            {
                if(testResult.Executed)
                {
                    testRunCount++;

                    if(testResult.IsFailure)
                    {
                        failureCount++;
                        Console.Write("F");
                        if ( debugger )
                        {
                            messages.Add( string.Format( "{0}) {1} :", failureCount, testResult.Test.TestName ) );
                            messages.Add( testResult.Message.Trim( Environment.NewLine.ToCharArray() ) );

                            string stackTrace = StackTraceFilter.Filter( testResult.StackTrace );
                            string[] trace = stackTrace.Split( System.Environment.NewLine.ToCharArray() );
                            foreach( string s in trace )
                            {
                                if ( s != string.Empty )
                                {
                                    string link = Regex.Replace( s.Trim(), @".* in (.*):line (.*)", "$1($2)");
                                    messages.Add( string.Format( "at\n{0}", link ) );
                                }
                            }
                        }
                    }
                }
                else
                {
                    testIgnoreCount++;
                    Console.Write("N");
                }


                currentTestName = string.Empty;
            }

            public void TestStarted(TestMethod testCase)
            {
                currentTestName = testCase.TestName.FullName;
            }

            public void TestStarted(TestName testName)
            {
                currentTestName = testName.FullName;
            }


            public void SuiteStarted(TestName name)
            {
                if ( debugger && level++ == 0 )
                {
                    testRunCount = 0;
                    testIgnoreCount = 0;
                    failureCount = 0;
                    Trace.WriteLine( "################################ UNIT TESTS ################################" );
                    Trace.WriteLine( "Running tests in '" + name + "'..." );
                }
            }

            public void SuiteFinished(TestResult suiteResult)
            {
                if ( debugger && --level == 0)
                {
                    Trace.WriteLine( "############################################################################" );

                    if (messages.Count == 0)
                    {
                        Trace.WriteLine( "##############                 S U C C E S S               #################" );
                    }
                    else
                    {
                        Trace.WriteLine( "##############                F A I L U R E S              #################" );

                        foreach ( string s in messages )
                        {
                            Trace.WriteLine(s);
                        }
                    }

                    Trace.WriteLine( "############################################################################" );
                    Trace.WriteLine( "Executed tests : " + testRunCount );
                    Trace.WriteLine( "Ignored tests  : " + testIgnoreCount );
                    Trace.WriteLine( "Failed tests   : " + failureCount );
                    Trace.WriteLine( "Total time     : " + suiteResult.Time + " seconds" );
                    Trace.WriteLine( "############################################################################");
                }
            }

            public void UnhandledException( Exception exception )
            {
                string msg = string.Format( "##### Unhandled Exception while running {0}", currentTestName );

                // If we do labels, we already have a newline
                //if ( !options.labels ) writer.WriteLine();
                writer.WriteLine( msg );
                writer.WriteLine( exception.ToString() );

                if ( debugger )
                {
                    Trace.WriteLine( msg );
                    Trace.WriteLine( exception.ToString() );
                }
            }

            public void TestOutput( TestOutput output)
            {
            }
        }

        #endregion

        public static bool IsWindows()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32Windows:
                case System.PlatformID.Win32S:
                case PlatformID.Win32NT:
                    return true;
            }
            return false;
        }
    }

    public class TestConsoleAccess: MarshalByRefObject, ITestConsoleAccess
    {
        StringBuilder mBuilder = new StringBuilder();

        public void WriteLine(string s)
        {
#if NET_2_0
            mBuilder.AppendLine(s);
#else
            mBuilder.Append(s + "\n");
#endif
            Console.WriteLine(s);
        }

        public void Write(char[] buf)
        {
            mBuilder.Append(buf);
            Console.Write(buf);
        }

        public void Write(char[] buf, int index, int count)
        {
            mBuilder.Append(buf, index, count);
            Console.Write(buf, index, count);
        }

        public string GetTestOutput()
        {
            return mBuilder.ToString();
        }
    }
}
