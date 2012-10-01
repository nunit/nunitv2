using System.Threading.Tasks;
using NUnit.Framework;

namespace nunit.core.tests.net45
{
	[Ignore("Run via code")]
	internal class DummyFixture
	{
		[Test]
		public async void Void()
		{
			
		}

		[Test]
		public async Task PlainTask()
		{
			await Task.Yield();
		}

		[Test]
		public async Task<int> TaskWithResult()
		{
			return await Task.FromResult(1);
		}

		[Test]
		public Task<int> NonAsyncTaskWithResult()
		{
			return Task.FromResult(1);
		}

		[Test]
		public Task NonAsyncTask()
		{
			return Task.Delay(0);
		}
	}
}