#if NET_3_5 || NET_4_0 || NET_4_5
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace nunit.framework.tests.net45
{
	[TestFixture]
	public class ThrowsTests
	{
		private readonly TestDelegate _noThrowsVoid = new TestDelegate(async () => await Task.Yield());
		private readonly ActualValueDelegate<Task> _noThrowsAsyncTask = async () => await Task.Yield();
		private readonly ActualValueDelegate<Task<int>> _noThrowsAsyncGenericTask = async () => await ReturnOne();
		private readonly TestDelegate _throwsAsyncVoid = new TestDelegate(async () => await ThrowAsyncTask());
		private readonly TestDelegate _throwsSyncVoid = new TestDelegate(async () => { throw new InvalidOperationException(); });
		private readonly ActualValueDelegate<Task> _throwsAsyncTask = async () => await ThrowAsyncTask();
		private readonly ActualValueDelegate<Task<int>> _throwsAsyncGenericTask = async () => await ThrowAsyncGenericTask();

		private static ThrowsConstraint ThrowsInvalidOperationExceptionConstraint
		{
			get { return new ThrowsConstraint(new ExactTypeConstraint(typeof(InvalidOperationException))); }
		}

		[Test]
		public void ThrowsConstraintVoid()
		{
			Assert.IsTrue(ThrowsInvalidOperationExceptionConstraint.Matches(_throwsAsyncVoid));
		}

		[Test]
		public void ThrowsConstraintVoidRunSynchronously()
		{
			Assert.IsTrue(ThrowsInvalidOperationExceptionConstraint.Matches(_throwsSyncVoid));
		}

		[Test]
		public void ThrowsConstraintAsyncTask()
		{
			Assert.IsTrue(ThrowsInvalidOperationExceptionConstraint.Matches(_throwsAsyncTask));
		}

		[Test]
		public void ThrowsConstraintAsyncGenericTask()
		{
			Assert.IsTrue(ThrowsInvalidOperationExceptionConstraint.Matches(_throwsAsyncGenericTask));
		}

		[Test]
		public void ThrowsNothingConstraintVoidSuccess()
		{
			Assert.IsTrue(new ThrowsNothingConstraint().Matches(_noThrowsVoid));
		}

		[Test]
		public void ThrowsNothingConstraintVoidFailure()
		{
			Assert.IsFalse(new ThrowsNothingConstraint().Matches(_throwsAsyncVoid));
		}

		[Test]
		public void ThrowsNothingConstraintTaskVoidSuccess()
		{
			Assert.IsTrue(new ThrowsNothingConstraint().Matches(_noThrowsAsyncTask));
		}

		[Test]
		public void ThrowsNothingConstraintTaskFailure()
		{
			Assert.IsFalse(new ThrowsNothingConstraint().Matches(_throwsAsyncTask));
		}

		[Test]
		public void AssertThrowsVoid()
		{
			Assert.Throws(typeof(InvalidOperationException), _throwsAsyncVoid);
		}

		[Test]
		public void AssertThatThrowsVoid()
		{
			Assert.That(_throwsAsyncVoid, Throws.TypeOf<InvalidOperationException>());
		}
		
		[Test]
		public void AssertThatThrowsTask()
		{
			Assert.That(_throwsAsyncTask, Throws.TypeOf<InvalidOperationException>());
		}

		[Test]
		public void AssertThatThrowsGenericTask()
		{
			Assert.That(_throwsAsyncGenericTask, Throws.TypeOf<InvalidOperationException>());
		}

		[Test]
		public void AssertThatThrowsNothingVoidSuccess()
		{
			Assert.That(_noThrowsVoid, Throws.Nothing);
		}

		[Test]
		public void AssertThatThrowsNothingTaskSuccess()
		{
			Assert.That(_noThrowsAsyncTask, Throws.Nothing);
		}

		[Test]
		public void AssertThatThrowsNothingGenericTaskSuccess()
		{
			Assert.That(_noThrowsAsyncGenericTask, Throws.Nothing);
		}

		[Test]
		public void AssertThatThrowsNothingVoidFailure()
		{
			Assert.Throws<AssertionException>(() => Assert.That(_throwsAsyncVoid, Throws.Nothing));
		}

		[Test]
		public void AssertThatThrowsNothingTaskFailure()
		{
			Assert.Throws<AssertionException>(() => Assert.That(_throwsAsyncTask, Throws.Nothing));
		}

		[Test]
		public void AssertThatThrowsNothingGenericTaskFailure()
		{
			Assert.Throws<AssertionException>(() => Assert.That(_throwsAsyncGenericTask, Throws.Nothing));
		}

		[Test]
		public void AssertThrowsAsync()
		{
			Assert.Throws<InvalidOperationException>(_throwsAsyncVoid);
		}

		[Test]
		public void AssertThrowsSync()
		{
			Assert.Throws<InvalidOperationException>(_throwsSyncVoid);
		}

		private static async Task ThrowAsyncTask()
		{
			await ReturnOne();
			throw new InvalidOperationException();
		}

		private static async Task<int> ThrowAsyncGenericTask()
		{
			await ThrowAsyncTask();
			return await ReturnOne();
		}

		private static Task<int> ReturnOne()
		{
			return Task.Run(() => 1);
		}
	}
}
#endif