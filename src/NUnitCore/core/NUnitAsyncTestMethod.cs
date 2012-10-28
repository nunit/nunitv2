using System.Reflection;

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
				var result = base.RunTestMethod();

				try
				{
					return region.WaitForPendingOperationsToComplete(result);
				}
				catch (AsyncInvocationException e)
				{
					throw new NUnitException("Rethrown", e.InnerException);
				}
			}
        }
    }
}