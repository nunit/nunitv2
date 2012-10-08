#if NET_3_5 || NET_4_0 || NET_4_5
using System.Collections;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Framework;
using test_assembly_net45;

namespace nunit.core.tests.net45
{
	[TestFixture]
	public class NUnitTestCaseBuilderTests
	{
		private NUnitTestCaseBuilder _sut;

		[SetUp]
		public void Setup()
		{
			_sut = new NUnitTestCaseBuilder();
		}

		public IEnumerable AsyncTestsSource
		{
			get
			{
				yield return new object[] { Method("AsyncVoidTest"), RunState.Runnable };
				yield return new object[] { Method("AsyncTaskTest"), RunState.Runnable };
				yield return new object[] { Method("AsyncTaskTTest"), RunState.NotRunnable };
			}
		}

		public IEnumerable AsyncTestCasesSource
		{
			get
			{
				yield return new object[] { Method("AsyncVoidTestCaseWithResultCheck"), RunState.NotRunnable };
				yield return new object[] { Method("AsyncTaskTestCaseWithResultCheck"), RunState.NotRunnable };
				yield return new object[] { Method("AsyncTaskTTestCaseWithResultCheck"), RunState.Runnable };
				yield return new object[] { Method("AsyncVoidTestCaseWithoutResultCheck"), RunState.Runnable };
				yield return new object[] { Method("AsyncTaskTestCaseWithoutResultCheck"), RunState.Runnable };
				yield return new object[] { Method("AsyncTaskTTestCaseWithoutResultCheck"), RunState.NotRunnable };
				yield return new object[] { Method("AsyncTaskTTestCaseExpectedExceptionWithoutResultCheck"), RunState.Runnable };
			}
		}

		[TestCaseSource("AsyncTestsSource")]
		public void AsyncTests(MethodInfo method, RunState state)
		{
			var built = _sut.BuildFrom(method);

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(state));
		}

		[TestCaseSource("AsyncTestCasesSource")]
		public void AsyncTestCases(MethodInfo method, RunState state)
		{
			var built = _sut.BuildFrom(method);

			var testMethod = built.Tests[0] as NUnitAsyncTestMethod;

			Assert.IsNotNull(testMethod);

			Assert.That(testMethod.RunState, Is.EqualTo(state));
		}

		[Test]
		public void Non_async_task()
		{
			var built = _sut.BuildFrom(Method("NonAsyncTask"));

			Assert.That(built, Is.Not.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.NotRunnable));
		}

		[Test]
		public void Non_async_task_with_result()
		{
			var built = _sut.BuildFrom(Method("NonAsyncTaskWithResult"));

			Assert.That(built, Is.Not.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.NotRunnable));
		}

		public MethodInfo Method(string name)
		{
			return typeof (AsyncDummyFixture).GetMethod(name);
		}
	}
}
#endif