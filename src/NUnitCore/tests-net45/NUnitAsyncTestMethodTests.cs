#if NET_3_5 || NET_4_0 || NET_4_5
using System;
using System.Collections;
using System.Diagnostics;
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
				yield return new object[] { Method("VoidTestSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("VoidTestFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("VoidTestError"), ResultState.Error, 0 };
				yield return new object[] { Method("VoidTestExpectedException"), ResultState.Success, 0 };

				yield return new object[] { Method("TaskTestSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("TaskTestFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("TaskTestError"), ResultState.Error, 0 };
				yield return new object[] { Method("TaskTestExpectedException"), ResultState.Success, 0 };

				yield return new object[] { Method("TaskTTestCaseWithResultCheckSuccess"), ResultState.Success, 0 };
				yield return new object[] { Method("TaskTTestCaseWithResultCheckFailure"), ResultState.Failure, 0 };
				yield return new object[] { Method("TaskTTestCaseWithResultCheckError"), ResultState.Failure, 0 };
				yield return new object[] { Method("TaskTTestCaseWithResultCheckSuccessReturningNull"), ResultState.Success, 0 };
				yield return new object[] { Method("TaskTTestCaseWithoutResultCheckExpectedExceptionSuccess"), ResultState.Success, 0 };

				yield return new object[] { Method("NestedVoidTestSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("NestedVoidTestFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("NestedVoidTestError"), ResultState.Error, 0 };

				yield return new object[] { Method("NestedTaskTestSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("NestedTaskTestFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("NestedTaskTestError"), ResultState.Error, 0 };

				yield return new object[] { Method("VoidTestMultipleSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("VoidTestMultipleFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("VoidTestMultipleError"), ResultState.Error, 0 };

				yield return new object[] { Method("TaskTestMultipleSuccess"), ResultState.Success, 1 };
				yield return new object[] { Method("TaskTestMultipleFailure"), ResultState.Failure, 1 };
				yield return new object[] { Method("TaskTestMultipleError"), ResultState.Error, 0 };
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

			var method = _builder.BuildFrom(Method("VoidAssertSynchrnoizationContext"));

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