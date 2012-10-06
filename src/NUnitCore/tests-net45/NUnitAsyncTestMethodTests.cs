#if NET_3_5 || NET_4_0 || NET_4_5
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Framework;
using test_assembly_net45;

namespace nunit.core.tests.net45
{
	[TestFixture]
	public class NUnitAsyncTestMethodTests
	{
		private NUnitTestCaseBuilder _builder;

		[SetUp]
		public void Setup()
		{
			_builder = new NUnitTestCaseBuilder();
		}

		public IEnumerable TestCases
		{
			get
			{
				yield return new object[] { Method("AsyncVoidSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("AsyncVoidFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("AsyncVoidError"), ResultState.Error, 0 };

				yield return new object[] { Method("AsyncTaskSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("AsyncTaskFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("AsyncTaskError"), ResultState.Error, 0 };

				yield return new object[] { Method("AsyncTaskResultSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("AsyncTaskResultFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("AsyncTaskResultError"), ResultState.Error, 0 };

				yield return new object[] { Method("AsyncTaskResultCheckSuccess"), ResultState.Success, 0 };
				//yield return new object[] { Method("AsyncVoidTestCaseWithParametersSuccess(0, 0)), ResultState.Success, 1 };
				yield return new object[] { Method("AsyncTaskResultCheckSuccessReturningNull"), ResultState.Success, 0 };
				yield return new object[] { Method("AsyncTaskResultCheckFailure"), ResultState.Failure, 0 };
				yield return new object[] { Method("AsyncTaskResultCheckError"), ResultState.Failure, 0 };

				yield return new object[] { Method("AsyncVoidExpectedException"), ResultState.Success, 0 };
				yield return new object[] { Method("AsyncTaskExpectedException"), ResultState.Success, 0 };
				yield return new object[] { Method("AsyncTaskResultExpectedException"), ResultState.Success, 0 };

				yield return new object[] { Method("NestedAsyncVoidSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("NestedAsyncVoidFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("NestedAsyncVoidError"), ResultState.Error, 0 };

				yield return new object[] { Method("NestedAsyncTaskSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("NestedAsyncTaskFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("NestedAsyncTaskError"), ResultState.Error, 0 };

				yield return new object[] { Method("AsyncVoidMultipleSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("AsyncVoidMultipleFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("AsyncVoidMultipleError"), ResultState.Error, 0 };

				yield return new object[] { Method("AsyncTaskMultipleSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("AsyncTaskMultipleFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("AsyncTaskMultipleError"), ResultState.Error, 0 };
			}
		}

		[Test]
		[TestCaseSource("TestCases")]
		public void RunTests(MethodInfo testMethod, ResultState resultState, int assertionCount)
		{
			var method = _builder.BuildFrom(testMethod);

			var result = method.Run(new NullListener(), TestFilter.Empty);

			Assert.That(result.Executed, Is.True, "Was not executed");
			Assert.That(result.ResultState, Is.EqualTo(resultState), "Wrong result state");
			Assert.That(result.AssertCount, Is.EqualTo(assertionCount), "Wrong assertion count");
		}

		[Test]
		public void SynchronizationContextSwitching()
		{
			var context = new CustomSynchronizationContext();

			SynchronizationContext.SetSynchronizationContext(context);

			var method = _builder.BuildFrom(Method("AsyncVoidAssertSynchrnoizationContext"));

			var result = method.Run(new NullListener(), TestFilter.Empty);

			Assert.AreSame(context, SynchronizationContext.Current);
			Assert.That(result.Executed, Is.True, "Was not executed");
			Assert.That(result.ResultState, Is.EqualTo(ResultState.Success), "Wrong result state");
			Assert.That(result.AssertCount, Is.EqualTo(1), "Wrong assertion count");
		}

		private static MethodInfo Method(string name)
		{
			return typeof (AsyncRealFixture).GetMethod(name);
		}

		public class CustomSynchronizationContext : SynchronizationContext
		{
		}
	}
}
#endif