using System;
using System.Threading;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
	public class AfterConstraintTest : ConstraintTestBase
	{
		private static bool value;

		[SetUp]
		public void SetUp()
		{
			theConstraint = new AfterConstraint(new EqualConstraint(true), 500);
			expectedDescription = "True after 500 millisecond delay";
			stringRepresentation = "<after 500 <equal True>>";

            value = false;
            SetValueTrueAfterDelay(300);
		}

        object[] SuccessData = new object[] { true };
        object[] FailureData = new object[] { false, 0, null };
		string[] ActualValues = new string[] { "False", "0", "null" };

		object[] InvalidData = new object[] { InvalidDelegate };

        ActualValueDelegate[] SuccessDelegates = new ActualValueDelegate[] { DelegateReturningValue };
        ActualValueDelegate[] FailureDelegates = new ActualValueDelegate[] { DelegateReturningFalse, DelegateReturningZero };

        [Test, TestCaseSource("SuccessDelegates")]
        public void SucceedsWithGoodDelegates(ActualValueDelegate del)
        {
            Assert.That(theConstraint.Matches(del));
        }

        [Test,TestCaseSource("FailureDelegates")]
        public void FailsWithBadDelegates(ActualValueDelegate del)
        {
            Assert.IsFalse(theConstraint.Matches(del));
        }

        [Test]
        public void SimpleTest()
        {
            SetValueTrueAfterDelay(500);

            Assert.That(value, Is.False);
            Assert.That(DelegateReturningValue, new AfterConstraint(new EqualConstraint(true), 5000, 200));
        }

        [Test]
        public void SimpleTestUsingReference()
        {
            SetValueTrueAfterDelay(500);

            Assert.That(value, Is.False);
            Assert.That(ref value, new AfterConstraint(new EqualConstraint(true), 5000, 200));
        }

        [Test]
        public void ThatOverload_ZeroDelayIsAllowed()
        {
            Assert.That(DelegateReturningZero, new AfterConstraint(new EqualConstraint(0), 0));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ThatOverload_DoesNotAcceptNegativeDelayValues()
        {
            Assert.That(DelegateReturningZero, new AfterConstraint(new EqualConstraint(0), -1));
        }

		private void SetValueTrueAfterDelay(int delay)
		{
			new Timer( SetValueTrueDelegate, null, delay, Timeout.Infinite);
		}

		private static void MethodReturningVoid() { }
		private static TestDelegate InvalidDelegate = new TestDelegate(MethodReturningVoid);

		private static object MethodReturningValue() { return value; }
		private static ActualValueDelegate DelegateReturningValue = new ActualValueDelegate(MethodReturningValue);

		private static object MethodReturningFalse() { return false; }
		private static ActualValueDelegate DelegateReturningFalse = new ActualValueDelegate(MethodReturningFalse);

		private static object MethodReturningZero() { return 0; }
		private static ActualValueDelegate DelegateReturningZero = new ActualValueDelegate(MethodReturningZero);

		private static void MethodSetsValueTrue(object state) { value = true; }
		private TimerCallback SetValueTrueDelegate = new TimerCallback(MethodSetsValueTrue);
	}
}
