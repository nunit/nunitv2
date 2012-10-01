using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Framework;

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
			var built = _sut.BuildFrom(Reflection.GetMethod<DummyFixture>(f => f.Void()));

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.Runnable));
		}

		[Test]
		public void Async_task()
		{
			var built = _sut.BuildFrom(Reflection.GetMethod<DummyFixture>(f => f.PlainTask()));

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.Runnable));
		}

		[Test]
		public void Async_task_with_result()
		{
			var built = _sut.BuildFrom(Reflection.GetMethod<DummyFixture>(f => f.TaskWithResult()));

			Assert.That(built, Is.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.Runnable));
		}

		[Test]
		public void Non_async_task()
		{
			var built = _sut.BuildFrom(Reflection.GetMethod<DummyFixture>(f => f.NonAsyncTask()));

			Assert.That(built, Is.Not.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.NotRunnable));
		}

		[Test]
		public void Non_async_task_with_result()
		{
			var built = _sut.BuildFrom(Reflection.GetMethod<DummyFixture>(f => f.NonAsyncTaskWithResult()));

			Assert.That(built, Is.Not.InstanceOf<NUnitAsyncTestMethod>());
			Assert.That(built.RunState, Is.EqualTo(RunState.NotRunnable));
		}
	}
}