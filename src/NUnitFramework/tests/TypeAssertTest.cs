using System;
using NUnit.Framework;

namespace NUnit.Extensions.Tests
{
	[TestFixture()]
	public class TypeAssertTest
	{
		[Test()]
		public void Implements()
		{
			TypeAssert.Implements(typeof(System.Runtime.Serialization.ISerializable),new System.ApplicationException("Bad News"));
			TypeAssert.Implements(typeof(System.Runtime.Serialization.ISerializable),new System.ApplicationException("Bad News"),"Type Failure Message");
			TypeAssert.Implements(typeof(System.Runtime.Serialization.ISerializable),new System.ApplicationException("Bad News"),"Type Failure Message",null);
		}

		[Test(), ExpectedException( typeof( AssertionException ) )]
		public void ImplementsFails()
		{
			TypeAssert.Implements(typeof(System.IServiceProvider),new System.Exception("Bad News"));
		}

		[Test()]
		public void IsSubclassOf()
		{
			TypeAssert.IsSubclassOf(typeof(System.Exception),new System.ApplicationException("Bad News"));
			TypeAssert.IsSubclassOf(typeof(System.Exception),new System.ApplicationException("Bad News"),"Type Failure Message");
			TypeAssert.IsSubclassOf(typeof(System.Exception),new System.ApplicationException("Bad News"),"Type Failure Message",null);
		}

		[Test(), ExpectedException( typeof( AssertionException ) )]
		public void IsSubclassOfFails()
		{
			TypeAssert.IsSubclassOf(typeof(System.Data.DataSet),new System.Exception("Bad News"));
		}

		[Test()]
		public void IsType()
		{
			TypeAssert.IsType(typeof(System.String),"abc123");
			TypeAssert.IsType(typeof(System.String),"abc123","Type Failure Message");
			TypeAssert.IsType(typeof(System.String),"abc123","Type Failure Message",null);
		}

		[Test(), ExpectedException( typeof( AssertionException ) )]
		public void IsTypeFails()
		{
			TypeAssert.IsType(typeof(System.Object),"abc123");
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

		[Test(), ExpectedException( typeof( AssertionException ) )]
		public void IsAssignableFromFails()
		{
			int [] array10 = new int [10];
			int [,] array2 = new int[2,2];

			TypeAssert.IsAssignableFrom(array2.GetType(),array10);
		}

	}
}
