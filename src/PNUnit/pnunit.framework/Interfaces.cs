using System;

using NUnit.Core;

namespace PNUnit.Framework
{

    public class Names
    {
        public const string PNUnitAgentServiceName = "IPNUnitAgent";
        public const string ServerBarrier = "SERVERSTART";
        public const string EndBarrier = "ENDBARRIER";
    }

    public interface ITestConsoleAccess
    {
        void WriteLine(string s);
        void Write(char[] buf);
        void Write(char[] buf, int index, int count);
    }

    public interface IPNUnitServices
    {
        void NotifyResult(string TestName, TestResult result);

        void InitBarriers(string TestName);
        void InitBarrier(string TestName, string barrier);
        void InitBarrier(string TestName, string barrier, int Max);
        void EnterBarrier(string barrier);
    }

	[Serializable]
	public class PNUnitTestInfo
	{
		public string TestName;
		public string AssemblyName;
		public string TestToRun;
		public string[] TestParams;
		public IPNUnitServices Services;
		public string StartBarrier;
		public string EndBarrier;
		public string[] WaitBarriers;

		public PNUnitTestInfo(string TestName, string AssemblyName, 
			string TestToRun, string[] TestParams, IPNUnitServices Services)
		{
			this.TestName = TestName;
			this.AssemblyName = AssemblyName;
			this.TestToRun = TestToRun;
			this.TestParams = TestParams;
			this.Services = Services;
		}

		public PNUnitTestInfo(string TestName, string AssemblyName, 
			string TestToRun, string[] TestParams, IPNUnitServices Services, string StartBarrier, string EndBarrier, string[] WaitBarriers)
		{
			this.TestName = TestName;
			this.AssemblyName = AssemblyName;
			this.TestToRun = TestToRun;
			this.TestParams = TestParams;
			this.Services = Services;
			this.StartBarrier = StartBarrier;
			this.EndBarrier = EndBarrier;
			this.WaitBarriers = WaitBarriers;
		}

	}

    public interface IPNUnitAgent
    {
        void RunTest(PNUnitTestInfo info);
    }

//    [Serializable]
//    public class PNUnitTestResult
//    {
//        public bool Executed;
//        public bool AllTestsExecuted;
//        public string Name;
//        public bool IsSuccess;
//        public bool IsFailure;
//        public string Description;
//        public double Time;
//        public string Message;
//        public string StackTrace;
//        public int AssertCount;
//
//        public PNUnitTestResult(TestResult result)
//        {
//            if( result == null )
//                return;
//            this.Executed = result.Executed;
//            this.Name = result.Name;
//            this.IsSuccess = result.IsSuccess;
//            this.IsFailure = result.IsFailure;
//            this.Description = result.Description;
//            this.Time = result.Time;
//            this.Message = result.Message;
//            this.StackTrace = result.StackTrace;
//            this.AssertCount  = result.AssertCount;                        
//        }
//
//        public PNUnitTestResult(Exception e)
//        {
//            this.Executed = false;
//            this.AllTestsExecuted = false;
//            this.AssertCount = 0;
//            this.Description = "Not controlled exception raised";
//            this.StackTrace = e.StackTrace;
//            this.Name = "";
//            this.IsSuccess = false;
//            this.IsFailure = true;
//            this.Time = 0;
//            this.Message = e.Message;
//        }
//    }

}

