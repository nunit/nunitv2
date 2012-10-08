using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Core;
using NUnit.Framework;

namespace test_assembly_net45
{
	public class AsyncRealFixture
	{
		[Test]
		public async void VoidTestSuccess()
		{
			var result = await ReturnOne();

			Assert.AreEqual(1, result);
		}

		[Test]
		public async void VoidTestFailure()
		{
			var result = await ReturnOne();

			Assert.AreEqual(2, result);
		}

		[Test]
		public async void VoidTestError()
		{
			await ThrowException();

			Assert.Fail("Should never get here");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public async void VoidTestExpectedException()
		{
			await ThrowException();
		}

		[Test]
		public async Task TaskTestSuccess()
		{
			var result = await ReturnOne();

			Assert.AreEqual(1, result);
		}

		[Test]
		public async Task TaskTestFailure()
		{
			var result = await ReturnOne();

			Assert.AreEqual(2, result);
		}

		[Test]
		public async Task TaskTestError()
		{
			await ThrowException();

			Assert.Fail("Should never get here");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public async Task TaskTestExpectedException()
		{
			await ThrowException();
		}

		[TestCase(Result = 1)]
		public async Task<int> TaskTTestCaseWithResultCheckSuccess()
		{
			return await ReturnOne();
		}

		[TestCase(Result = 2)]
		public async Task<int> TaskTTestCaseWithResultCheckFailure()
		{
			return await ReturnOne();
		}

		[TestCase(Result = 0)]
		public async Task<int> TaskTTestCaseWithResultCheckError()
		{
			return await ThrowException();
		}

		[TestCase(Result = null)]
		public async Task<object> TaskTTestCaseWithResultCheckSuccessReturningNull()
		{
			return await Task.Run(() => (object)null);
		}

		[TestCase(ExpectedException = typeof(InvalidOperationException))]
		public async Task<object> TaskTTestCaseWithoutResultCheckExpectedExceptionSuccess()
		{
			return await ThrowException();
		}

		[Test]
		public async void VoidAssertSynchrnoizationContext()
		{
			Assert.That(SynchronizationContext.Current, Is.InstanceOf<AsyncSynchronizationContext>());
			await Task.Yield();
		}

		[Test]
		public async void NestedVoidTestSuccess()
		{
			var result = await Task.Run(async () => await ReturnOne());

			Assert.AreEqual(1, result);
		}

		[Test]
		public async void NestedVoidTestFailure()
		{
			var result = await Task.Run(async () => await ReturnOne());

			Assert.AreEqual(2, result);
		}

		[Test]
		public async void NestedVoidTestError()
		{
			await Task.Run(async () => await ThrowException());

			Assert.Fail("Should not get here");
		}

		[Test]
		public async Task NestedTaskTestSuccess()
		{
			var result = await Task.Run(async () => await ReturnOne());

			Assert.AreEqual(1, result);
		}

		[Test]
		public async Task NestedTaskTestFailure()
		{
			var result = await Task.Run(async () => await ReturnOne());

			Assert.AreEqual(2, result);
		}

		[Test]
		public async Task NestedTaskTestError()
		{
			await Task.Run(async () => await ThrowException());

			Assert.Fail("Should never get here");
		}

		[Test]
		public async void VoidTestMultipleSuccess()
		{
			var result = await ReturnOne();

			Assert.AreEqual(await ReturnOne(), result);
		}

		[Test]
		public async void VoidTestMultipleFailure()
		{
			var result = await ReturnOne();

			Assert.AreEqual(await ReturnOne() + 1, result);
		}

		[Test]
		public async void VoidTestMultipleError()
		{
			var result = await ReturnOne();
			await ThrowException();

			Assert.Fail("Should never get here");
		}

		[Test]
		public async Task TaskTestMultipleSuccess()
		{
			var result = await ReturnOne();

			Assert.AreEqual(await ReturnOne(), result);
		}

		[Test]
		public async Task TaskTestMultipleFailure()
		{
			var result = await ReturnOne();

			Assert.AreEqual(await ReturnOne() + 1, result);
		}

		[Test]
		public async Task TaskTestMultipleError()
		{
			var result = await ReturnOne();
			await ThrowException();

			Assert.Fail("Should never get here");
		}

		private static Task<int> ReturnOne()
		{
			return Task.Run(() => 1);
		}

		private static Task<int> ThrowException()
		{
			return Task.Run(() =>
			{
				throw new InvalidOperationException();
				return 1;
			});
		}
	}
}