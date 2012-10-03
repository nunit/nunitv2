using System.Threading.Tasks;
using NUnit.Framework;

namespace test_assembly_net45
{
	public class AsyncDummyFixture
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

		[TestCase(Result = 1)]
		public async Task AsyncTaskTestCase()
		{
			await Task.Run(() => 1);
		}

		[TestCase(Result = 1)]
		public async void AsyncVoidTestCase()
		{
			await Task.Run(() => 1);
		}

		[TestCase(Result = 1)]
		public async Task<int> AsyncTaskWithResultTestCase()
		{
			return await Task.Run(() => 1);
		}
	}
}