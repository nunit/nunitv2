
namespace NUnit.Tests
{
	using System;
	using System.Threading;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for TestDelegate.
	/// </summary>
	/// 
	[TestFixture]
	public class TestDelegateFixture
	{
		internal class TestDelegate 
		{ 
			public bool delegateCalled = false;

			public delegate void CallBackFunction(); 

			public TestDelegate() 
			{ 
				new CallBackFunction 
					(DoSomething).BeginInvoke 
					(null,null); 
			} 

			public void DoSomething() 
			{ 
				delegateCalled = true;
			} 
		} 

		[Test]
		public void DelegateTest()
		{
			TestDelegate testDelegate = new TestDelegate(); 
			Yield();
			Assertion.Assert(testDelegate.delegateCalled);
		}

		private void Yield()
		{
			Thread currentThread = Thread.CurrentThread;
			currentThread.Join(1000);
		}
	}
} 

