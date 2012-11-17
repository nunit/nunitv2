#if NET_3_5 || NET_4_0 || NET_4_5
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace nunit.framework.tests.net45
{
	[TestFixture]
	public class AssertAssumeThatTests
	{
		[Test]
		public void AssertThatSuccess()
		{
			Assert.That(async () => await One(), Is.EqualTo(1));
		}

		[Test]
		public void AssertThatFailure()
		{
			var exception = Assert.Throws<AssertionException>(() =>
				Assert.That(async () => await One(), Is.EqualTo(2)));
		}

		[Test]
		public void AssertThatErrorTask()
		{
			var exception = Assert.Throws<InvalidOperationException>(() => 
				Assert.That(async () => await ThrowExceptionTask(), Is.EqualTo(1)));

			Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionTask"));
		}

		[Test]
		public void AssertThatErrorGenericTask()
		{
			var exception = Assert.Throws<InvalidOperationException>(() => 
				Assert.That(async () => await ThrowExceptionGenericTask(), Is.EqualTo(1)));

			Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
		}

		[Test]
		public void AssertThatErrorVoid()
		{
			var exception = Assert.Throws<InvalidOperationException>(() =>
				Assert.That(async () => { await ThrowExceptionGenericTask(); }, Is.EqualTo(1)));

			Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
		}

		[Test]
		public void AssumeThatSuccess()
		{
			Assume.That(async () => await One(), Is.EqualTo(1));
		}

		[Test]
		public void AssumeThatFailure()
		{
			var exception = Assert.Throws<InconclusiveException>(() =>
				Assume.That(async () => await One(), Is.EqualTo(2)));
		}

		[Test]
		public void AssumeThatError()
		{
			var exception = Assert.Throws<InvalidOperationException>(() =>
				Assume.That(async () => await ThrowExceptionGenericTask(), Is.EqualTo(1)));

			Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
		}

		private static Task<int> One()
		{
			return Task.Run(() => 1);
		}

		private static async Task<int> ThrowExceptionGenericTask()
		{
			await One();
			throw new InvalidOperationException();
		}

		private static async Task ThrowExceptionTask()
		{
			await One();
			throw new InvalidOperationException();
		}

		private static async void ThrowExceptionVoid()
		{
			await One();
			throw new InvalidOperationException();
		}
	}
}
#endif