using System;
namespace NUnit.Framework.Tests
{
	[TestFixture()]
	public class TypeAssertTests
	{
		[Test]
		public void IsInstanceOfType()
		{
			Assert.IsInstanceOfType(typeof(System.Exception), new ApplicationException() );
		}

		[Test]
		public void IsInstanceOfTypeFails()
		{
			InstanceOfTypeAsserter asserter = new InstanceOfTypeAsserter(
				typeof(System.Int32), "abc123", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual(
				"\r\n\texpected: Object to be instance of System.Int32\r\n\t but was: System.String",
				asserter.Message );
		}

		[Test]
		public void IsNotInstanceOfType()
		{
			Assert.IsNotInstanceOfType(typeof(System.Int32), "abc123" );
		}

		[Test]
		public void IsNotInstanceOfTypeFails()
		{
			NotInstanceOfTypeAsserter asserter = new NotInstanceOfTypeAsserter(
				typeof(System.Exception), new System.ApplicationException(), null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual(
				"\r\n\texpected: Object not an instance of System.Exception\r\n\t but was: System.ApplicationException",
				asserter.Message );
		}

		[Test()]
		public void IsAssignableFrom()
		{
			int [] array10 = new int [10];
			int [] array2 = new int[2];

			Assert.IsAssignableFrom(array2.GetType(),array10);
			Assert.IsAssignableFrom(array2.GetType(),array10,"Type Failure Message");
			Assert.IsAssignableFrom(array2.GetType(),array10,"Type Failure Message",null);
		}

		[Test]
		public void IsAssignableFromFails()
		{
			int [] array10 = new int [10];
			int [,] array2 = new int[2,2];

			AssignableFromAsserter asserter = new AssignableFromAsserter(
				array2.GetType(), array10, null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"\r\n\texpected: Type assignable from System.Int32[,]\r\n\t but was: System.Int32[]", 
				asserter.Message );
		}

		[Test()]
		public void IsNotAssignableFrom()
		{
			int [] array10 = new int [10];
			int [,] array2 = new int[2,2];

			Assert.IsNotAssignableFrom(array2.GetType(),array10);
			Assert.IsNotAssignableFrom(array2.GetType(),array10,"Type Failure Message");
			Assert.IsNotAssignableFrom(array2.GetType(),array10,"Type Failure Message",null);
		}

		[Test]
		public void IsNotAssignableFromFails()
		{
			int [] array10 = new int [10];
			int [] array2 = new int[2];

			NotAssignableFromAsserter asserter = new NotAssignableFromAsserter(
				array2.GetType(), array10, null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"\r\n\texpected: Type not assignable from System.Int32[]\r\n\t but was: System.Int32[]", 
				asserter.Message );
		}
	}
}
