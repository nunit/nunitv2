// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Threading;
#if CLR_2_0 || CLR_4_0
using ActualValueDelegate = NUnit.Framework.Constraints.ActualValueDelegate<object>;
#else
using ActualValueDelegate = NUnit.Framework.Constraints.ActualValueDelegate;
#endif

namespace NUnit.Framework.Constraints
{
    [TestFixture]
	public class DelayedConstraintTest : ConstraintTestBase
	{
		private static bool _value;

		[SetUp]
		public void SetUp()
		{
			theConstraint = new DelayedConstraint(new EqualConstraint(true), 500);
			expectedDescription = "True after 500 millisecond delay";
			stringRepresentation = "<after 500 <equal True>>";

            _value = false;
		}

        internal object[] SuccessData = new object[] { true };
        internal object[] FailureData = new object[] { false, 0, null };
		internal string[] ActualValues = new string[] { "False", "0", "null" };

		internal object[] InvalidData = new object[] { InvalidDelegate };

        internal ActualValueDelegate[] SuccessDelegates = new ActualValueDelegate[] { DelegateReturningBoolAsObject };
        internal ActualValueDelegate[] FailureDelegates = new ActualValueDelegate[] { DelegateReturningFalseAsObject, DelegateReturningZeroAsObject };

        [Test, TestCaseSource("SuccessDelegates")]
        public void SucceedsWithGoodDelegates(ActualValueDelegate del)
        {
            SetValueTrueAfterDelay(300);
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
            Assert.That(DelegateReturningBoolAsObject, new DelayedConstraint(new EqualConstraint(true), 5000, 200));
        }

        [Test]
        public void SimpleTestUsingReference()
        {
            SetValueTrueAfterDelay(500);
            Assert.That(ref _value, new DelayedConstraint(new EqualConstraint(true), 5000, 200));
        }

        [Test]
        public void ThatOverload_ZeroDelayIsAllowed()
        {
            Assert.That(DelegateReturningZeroAsObject, new DelayedConstraint(new EqualConstraint(0), 0));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ThatOverload_DoesNotAcceptNegativeDelayValues()
        {
            Assert.That(DelegateReturningZeroAsObject, new DelayedConstraint(new EqualConstraint(0), -1));
        }

#if CLR_2_0 || CLR_4_0
		[Test]
		public void SimpleTestBoolDelegate()
		{
			SetValueTrueAfterDelay(500);
			Assert.That(DelegateReturningBool, new DelayedConstraint(new EqualConstraint(true), 5000, 200));
		}

		[Test]
		public void ThatOverload_ZeroDelayIsAllowed_IntDelegate()
		{
			Assert.That(DelegateReturningZero, new DelayedConstraint(new EqualConstraint(0), 0));
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void ThatOverload_DoesNotAcceptNegativeDelayValues_IntDelegate()
		{
			Assert.That(DelegateReturningZero, new DelayedConstraint(new EqualConstraint(0), -1));
		}

        [Test]
        public void CanTestContentsOfList()
        {
            var worker = new System.ComponentModel.BackgroundWorker();
            var list = new System.Collections.Generic.List<int>();
            worker.RunWorkerCompleted += delegate { list.Add(1); };
            worker.DoWork += delegate { Thread.Sleep(1); };
            worker.RunWorkerAsync();
            Assert.That(list, Has.Count.EqualTo(1).After(5000, 100));
        }

        [Test]
        public void CanTestContentsOfRefList()
        {
            var worker = new System.ComponentModel.BackgroundWorker();
            var list = new System.Collections.Generic.List<int>();
            worker.RunWorkerCompleted += delegate { list.Add(1); };
            worker.DoWork += delegate { Thread.Sleep(1); };
            worker.RunWorkerAsync();
            Assert.That(ref list, Has.Count.EqualTo(1).After(5000, 100));
        }

        [Test]
        public void CanTestContentsOfDelegateReturningList()
        {
            var worker = new System.ComponentModel.BackgroundWorker();
            var list = new System.Collections.Generic.List<int>();
            worker.RunWorkerCompleted += delegate { list.Add(1); };
            worker.DoWork += delegate { Thread.Sleep(1); };
            worker.RunWorkerAsync();
            Assert.That(() => list, Has.Count.EqualTo(1).After(5000, 100));
        }

        [Test]
        public void CanTestInitiallyNullReference()
        {
            string statusString = null; // object starts off as null

            var worker = new System.ComponentModel.BackgroundWorker();
            worker.RunWorkerCompleted += delegate { statusString = "finished"; /* object non-null after work */ };
            worker.DoWork += delegate { Thread.Sleep(TimeSpan.FromSeconds(1)); /* simulate work */ };
            worker.RunWorkerAsync();

            Assert.That(ref statusString, Has.Length.GreaterThan(0).After(3000, 100));
        }

        [Test]
        public void CanTestInitiallyNullDelegate()
        {
            string statusString = null; // object starts off as null

            var worker = new System.ComponentModel.BackgroundWorker();
            worker.RunWorkerCompleted += delegate { statusString = "finished"; /* object non-null after work */ };
            worker.DoWork += delegate { Thread.Sleep(TimeSpan.FromSeconds(1)); /* simulate work */ };
            worker.RunWorkerAsync();

            Assert.That(() => statusString, Has.Length.GreaterThan(0).After(3000, 100));
        }
#endif

        private static int setValueTrueDelay;

		private void SetValueTrueAfterDelay(int delay)
		{
            setValueTrueDelay = delay;
            Thread thread = new Thread( SetValueTrueDelegate );
            thread.Start();
		}

		private static void MethodReturningVoid() { }
		private static TestDelegate InvalidDelegate = new TestDelegate(MethodReturningVoid);

		private static object MethodReturningBoolAsObject() { return _value; }
		private static ActualValueDelegate DelegateReturningBoolAsObject = new ActualValueDelegate(MethodReturningBoolAsObject);

		private static object MethodReturningFalseAsObject() { return false; }
		private static ActualValueDelegate DelegateReturningFalseAsObject = new ActualValueDelegate(MethodReturningFalseAsObject);

		private static object MethodReturningZeroAsObject() { return 0; }
		private static ActualValueDelegate DelegateReturningZeroAsObject = new ActualValueDelegate(MethodReturningZeroAsObject);

#if CLR_2_0 || CLR_4_0
		private static bool MethodReturningBool() { return _value; }
		private static ActualValueDelegate<bool> DelegateReturningBool = new ActualValueDelegate<bool>(MethodReturningBool);

		private static bool MethodReturningFalse() { return false; }
		private static ActualValueDelegate<bool> DelegateReturningFalse = new ActualValueDelegate<bool>(MethodReturningFalse);

		private static int MethodReturningZero() { return 0; }
		private static ActualValueDelegate<int> DelegateReturningZero = new ActualValueDelegate<int>(MethodReturningZero);
#endif

        private static void MethodSetsValueTrue()
        {
            Thread.Sleep(setValueTrueDelay);
            _value = true;
        }
		private ThreadStart SetValueTrueDelegate = new ThreadStart(MethodSetsValueTrue);
	}
}
