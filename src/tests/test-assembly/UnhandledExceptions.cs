using System;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    public class UnhandledExceptions
    {
        #region Normal
        [NUnit.Framework.Test]
        public void Normal()
        {
            throw new ApplicationException("Test exception");
        }
        #endregion Normal

        #region Threaded
        [NUnit.Framework.Test]
        public void Threaded()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Normal));
            thread.Start();
            System.Threading.Thread.Sleep(100);
        }
        #endregion Threaded

        #region ThreadedAndForget
        [NUnit.Framework.Test]
        public void ThreadedAndForget()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Normal));
            thread.Start();
        }
        #endregion ThreadedAndForget


        #region ThreadedAndWait
        [NUnit.Framework.Test]
        public void ThreadedAndWait()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Normal));
            thread.Start();
            thread.Join();
        }
        #endregion ThreadedAndWait
    }
}
