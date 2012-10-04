#if NET_3_5 || NET_4_0 || NET_4_5
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

		[Test]
		public void Async_void()
		{
			var built = _sut.BuildFrom(Method("Void"));

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.Runnable));
		}

		[Test]
		public void Async_task()
		{
			var built = _sut.BuildFrom(Method("PlainTask"));

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.Runnable));
		}

		[Test]
		public void Async_task_testcase_result_check()
		{
			var built = _sut.BuildFrom(Method("AsyncTaskTestCase"));

			var testMethod = built.Tests[0] as NUnitAsyncTestMethod;

			Assert.IsNotNull(testMethod);

			Assert.That(testMethod.RunState, Is.EqualTo(RunState.NotRunnable));
		}

		[Test]
		public void Async_void_testcase_result_check()
		{
			var built = _sut.BuildFrom(Method("AsyncTaskTestCase"));

			var testMethod = built.Tests[0] as NUnitAsyncTestMethod;

			Assert.IsNotNull(testMethod);

			Assert.That(testMethod.RunState, Is.EqualTo(RunState.NotRunnable));
		}

		[Test]
		public void Async_task_with_result_testcase_result_check()
		{
			var built = _sut.BuildFrom(Method("AsyncTaskWithResultTestCase"));

			var testMethod = built.Tests[0] as NUnitAsyncTestMethod;

			Assert.IsNotNull(testMethod);

			Assert.That(testMethod.RunState, Is.EqualTo(RunState.Runnable));
		}

		[Test]
		public void Async_task_with_result()
		{
			var built = _sut.BuildFrom(Method("TaskWithResult"));

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.Runnable));
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