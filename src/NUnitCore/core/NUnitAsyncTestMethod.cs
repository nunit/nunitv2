using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace NUnit.Core
{
    public class NUnitAsyncTestMethod : NUnitTestMethod
    {
        public NUnitAsyncTestMethod(MethodInfo method) : base(method)
        {
        }

        protected override object RunTestMethod(TestResult testResult)
        {
            if (method.ReturnType == typeof (void))
            {
                return RunVoidAsyncMethod(testResult);
            }
            
            return RunTaskAsyncMethod(testResult);
        }

        private object RunTaskAsyncMethod(TestResult testResult)
        {
            try
            {
                object result = base.RunTestMethod(testResult);

                Reflect.InvokeMethod(method.ReturnType.GetMethod("Wait", new Type[0]), result);

                return result;
            }
            catch (NUnitException e)
            {
                if (e.InnerException != null && e.InnerException.GetType().FullName.Equals("System.AggregateException"))
                {
                    IList<Exception> inner = (IList<Exception>)e.InnerException.GetType().GetProperty("InnerExceptions").GetValue(e.InnerException, null);

                    throw new NUnitException("Rethrown", inner[0]);
                }

                throw;
            }
        }

        private object RunVoidAsyncMethod(TestResult testResult)
        {
            var previousContext = SynchronizationContext.Current;
            var newContext = new AsyncSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(newContext);

            try
            {
                object result = base.RunTestMethod(testResult);

                newContext.WaitForOperationCompleted();

                if (newContext.Exceptions.Count > 0)
                    throw new NUnitException("Rethrown", newContext.Exceptions[0]);

                return result;
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }
    }
}