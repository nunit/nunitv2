using System;
using System.Reflection;
using NUnit.Framework;

namespace NUnit.Core
{
    public class NUnitAsyncTestMethod : NUnitTestMethod
    {
	    public NUnitAsyncTestMethod(MethodInfo method) : base(method)
        {
        }

        protected override object RunTestMethod()
        {
			using (AsyncInvocationRegion region = AsyncInvocationRegion.Create(method))
			{
				object result = base.RunTestMethod();

				try
				{
					return region.WaitForPendingOperationsToComplete(result);
				}
				catch (Exception e)
				{
					throw new NUnitException("Rethrown", e);
				}
			}
        }
    }
}