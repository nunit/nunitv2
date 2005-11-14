using System;
namespace NUnit.Framework.Tests
{
	[TestFixture()]
	public class TypeAssertTest
	{
		[Test]
		public void Implements()
		{
			TypeAssert.Implements(typeof(System.Runtime.Serialization.ISerializable),new System.ApplicationException("Bad News"));
			TypeAssert.Implements(typeof(System.Runtime.Serialization.ISerializable),new System.ApplicationException("Bad News"),"Type Failure Message");
			TypeAssert.Implements(typeof(System.Runtime.Serialization.ISerializable),new System.ApplicationException("Bad News"),"Type Failure Message",null);
		}

		[Test]
		public void ImplementsFails()
		{
			ImplementsAsserter asserter = new ImplementsAsserter(
				typeof(System.IServiceProvider),new System.Exception("Bad News"), null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"\r\n\texpected: Type implementing System.IServiceProvider\r\n\t but was: System.Exception", 
				asserter.Message );
		}

		[Test]
		public void IsSubclassOf()
		{
			TypeAssert.IsSubclassOf(typeof(System.Exception),new System.ApplicationException("Bad News"));
			TypeAssert.IsSubclassOf(typeof(System.Exception),new System.ApplicationException("Bad News"),"Type Failure Message");
			TypeAssert.IsSubclassOf(typeof(System.Exception),new System.ApplicationException("Bad News"),"Type Failure Message",null);
		}

		[Test]
		public void IsSubclassOfFails()
		{
			IsSubclassOfAsserter asserter = new IsSubclassOfAsserter(
				typeof(System.ApplicationException),new System.Exception("Bad News"), null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( "\r\n\texpected: Subclass of System.ApplicationException\r\n\t but was: System.Exception", asserter.Message );
		}

		[Test]
		public void IsSubclassMessage()
		{
		}


		[Test]
		public void IsType()
		{
			TypeAssert.IsType(typeof(System.String),"abc123");
			TypeAssert.IsType(typeof(System.String),"abc123","Type Failure Message");
			TypeAssert.IsType(typeof(System.String),"abc123","Type Failure Message",null);
		}

		[Test]
		public void IsTypeFails()
		{
			IsTypeAsserter asserter = new IsTypeAsserter(
				typeof(System.Object), "abc123", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"\r\n\texpected: System.Object\r\n\t but was: System.String", 
				asserter.Message );
		}

		[Test()]
		public void IsAssignableFrom()
		{
			int [] array10 = new int [10];
			int [] array2 = new int[2];

			TypeAssert.IsAssignableFrom(array2.GetType(),array10);
			TypeAssert.IsAssignableFrom(array2.GetType(),array10,"Type Failure Message");
			TypeAssert.IsAssignableFrom(array2.GetType(),array10,"Type Failure Message",null);
		}

		[Test]
		public void IsAssignableFromFails()
		{
			int [] array10 = new int [10];
			int [,] array2 = new int[2,2];

			IsAssignableFromAsserter asserter = new IsAssignableFromAsserter(
				array2.GetType(), array10, null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"\r\n\texpected: Type assignable from System.Int32[,]\r\n\t but was: System.Int32[]", 
				asserter.Message );
		}

	}
}
