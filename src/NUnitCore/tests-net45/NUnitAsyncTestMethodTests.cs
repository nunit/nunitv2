using System;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Framework;


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
				yield return new object[] { Method(f => f.AsyncVoidSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.AsyncVoidFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.AsyncVoidError()), ResultState.Error, 0 };

				yield return new object[] { Method(f => f.AsyncTaskSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.AsyncTaskFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.AsyncTaskError()), ResultState.Error, 0 };

				yield return new object[] { Method(f => f.AsyncTaskResultSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.AsyncTaskResultFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.AsyncTaskResultError()), ResultState.Error, 0 };

				yield return new object[] { Method(f => f.AsyncTaskResultCheckSuccess()), ResultState.Success, 0 };
				yield return new object[] { Method(f => f.AsyncTaskResultCheckSuccessReturningNull()), ResultState.Success, 0 };
				yield return new object[] { Method(f => f.AsyncTaskResultCheckFailure()), ResultState.Failure, 0 };
				yield return new object[] { Method(f => f.AsyncTaskResultCheckError()), ResultState.Failure, 0 };

				yield return new object[] { Method(f => f.AsyncVoidExpectedException()), ResultState.Success, 0 };
				yield return new object[] { Method(f => f.AsyncTaskExpectedException()), ResultState.Success, 0 };
				yield return new object[] { Method(f => f.AsyncTaskResultExpectedException()), ResultState.Success, 0 };

				yield return new object[] { Method(f => f.NestedAsyncVoidSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.NestedAsyncVoidFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.NestedAsyncVoidError()), ResultState.Error, 0 };

				yield return new object[] { Method(f => f.NestedAsyncTaskSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.NestedAsyncTaskFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.NestedAsyncTaskError()), ResultState.Error, 0 };

				yield return new object[] { Method(f => f.AsyncVoidMultipleSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.AsyncVoidMultipleFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.AsyncVoidMultipleError()), ResultState.Error, 0 };

				yield return new object[] { Method(f => f.AsyncTaskMultipleSuccess()), ResultState.Success, 1 };
				yield return new object[] { Method(f => f.AsyncTaskMultipleFailure()), ResultState.Failure, 1 };
				yield return new object[] { Method(f => f.AsyncTaskMultipleError()), ResultState.Error, 0 };
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

			var method = _builder.BuildFrom(Method(f => f.AsyncVoidAssertSynchrnoizationContext()));

			var result = method.Run(new NullListener(), TestFilter.Empty);

			Assert.AreSame(context, SynchronizationContext.Current);
			Assert.That(result.Executed, Is.True, "Was not executed");
			Assert.That(result.ResultState, Is.EqualTo(ResultState.Success), "Wrong result state");
			Assert.That(result.AssertCount, Is.EqualTo(1), "Wrong assertion count");
		}

		private static MethodInfo Method(Expression<Action<RealFixture>> action)
		{
			return Reflection.GetMethod(action);
		}

		public class CustomSynchronizationContext : SynchronizationContext
		{
		}
	}
}