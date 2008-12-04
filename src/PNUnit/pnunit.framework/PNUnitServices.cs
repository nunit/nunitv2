using System;

namespace PNUnit.Framework
{
    [Serializable]
    public class PNUnitServices
    {
        private PNUnitTestInfo mInfo = null;
        private ITestConsoleAccess mConsole = null;
        private static PNUnitServices mInstance = null;

        // To be used only by the runner
        public PNUnitServices(object info, object consoleaccess)
        {
            mInfo = info as PNUnitTestInfo;
            mConsole = consoleaccess as ITestConsoleAccess;
            mInstance = this;
        }

        public static PNUnitServices Get()
        {
            if (mInstance == null)
            {
                throw new Exception("mInstance is null");
            }
            return mInstance;
        }

        private void CheckInfo()
        {
            if (mInfo == null)
                throw new Exception("TestInfo not initialized");
        }

        // IPNUnitServices

        public void InitBarriers()
        {
            CheckInfo();
            mInfo.Services.InitBarriers();
        }

        public void EnterBarrier(string barrier)
        {
            CheckInfo();
            mConsole.WriteLine(
                string.Format(">>>Test {0} entering barrier {1}",
                mInfo.TestName, barrier));
            mInfo.Services.EnterBarrier(barrier);
            mConsole.WriteLine(
                string.Format("<<<Test {0} leaving barrier {1}",
                mInfo.TestName, barrier));
        }

        public string[] GetTestWaitBarriers()
        {
            CheckInfo();
            return mInfo.WaitBarriers;
        }

        public string GetTestName()
        {
            CheckInfo();
            return mInfo.TestName;
        }

        public string[] GetTestParams()
        {
            CheckInfo();
            return mInfo.TestParams;
        }

        public void WriteLine(string s)
        {
            if (mConsole != null)
                mConsole.WriteLine(s);
        }

        public void Write(char[] buf)
        {
            if (mConsole != null)
                mConsole.Write(buf);
        }

        public void Write(char[] buf, int index, int count)
        {
            if (mConsole != null)
                mConsole.Write(buf, index, count);
        }

        public string GetTestStartBarrier()
        {
            CheckInfo();
            if (mInfo.StartBarrier == null || mInfo.StartBarrier == string.Empty)
                mInfo.StartBarrier = Names.ServerBarrier;
            return mInfo.StartBarrier;
        }

        public string GetTestEndBarrier()
        {
            CheckInfo();
            if (mInfo.EndBarrier == null || mInfo.EndBarrier == string.Empty)
                mInfo.EndBarrier = Names.EndBarrier;
            return mInfo.EndBarrier;
        }
    }
}
