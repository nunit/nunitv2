#if NET_3_5 || NET_4_0 || NET_4_5
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace nunit.core.tests.net45
{
	[TestFixture]
	public class ThrowsTests
	{
		[Test]
		public void ThrowsConstraintAsyncTask()
		{
			Assert.IsTrue(new ThrowsConstraint(new ExactTypeConstraint(typeof(InvalidOperationException)))
				.Matches(new TestDelegate(async () => await ThrowAsyncNonGenericTask())));
		}

		[Test]
		public void ThrowsConstraintAsyncGenericTask()
		{
			Assert.IsTrue(new ThrowsConstraint(new ExactTypeConstraint(typeof(InvalidOperationException)))
				.Matches(new TestDelegate(async () => { await ThrowAsyncGenericTask(); })));
		}

		[Test]
		public void ThrowsConstraintAsyncTaskRunSynchronously()
		{
#pragma warning disable 1998
			Assert.IsTrue(new ThrowsConstraint(new ExactTypeConstraint(typeof(InvalidOperationException)))
				.Matches(new TestDelegate(async () => { throw new InvalidOperationException(); })));
#pragma warning restore 1998
		}

		[Test]
		public void AssertThrows()
		{
			Assert.Throws(typeof(InvalidOperationException), async () => await ThrowAsyncNonGenericTask());
		}

		[Test]
		public void AssertThrowsGenericTask()
		{
			Assert.Throws(typeof(InvalidOperationException), async () => await ThrowAsyncGenericTask());
		}

		[Test]
		public void AssertThat()
		{
			Assert.That(async () => await ThrowAsyncNonGenericTask(), Throws.TypeOf<InvalidOperationException>());
		}

		[Test]
		public void AssertThatGenericTask()
		{
			Assert.That(async () => await ThrowAsyncGenericTask(), Throws.TypeOf<InvalidOperationException>());
		}

		[Test]
		public void AssertThrowsGeneric()
		{
			Assert.Throws<InvalidOperationException>(async () => await ThrowAsyncNonGenericTask());
		}

		[Test]
		public void AssertThrowsGenericWithGenericTask()
		{
			Assert.Throws<InvalidOperationException>(async () => await ThrowAsyncGenericTask());
		}

		private static async Task ThrowAsyncNonGenericTask()
		{
			await Task.Run(() => 1);
			throw new InvalidOperationException();
		}

		private static async Task<int> ThrowAsyncGenericTask()
		{
			await ThrowAsyncNonGenericTask();
			return await Task.Run(() => 1);
		}
	}
}
#endif