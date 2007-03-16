// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
//using NUnit.Framework.Syntax;

namespace NUnit.Framework.Tests
{
	[TestFixture()]
	public class TypeAssertTests : MessageChecker
	{
		[Test]
		public void ExactType()
		{
			Expect( "Hello", TypeOf( typeof(System.String) ) );
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void ExactTypeFails()
		{
			expectedMessage =
				"  Expected: <System.Int32>" + Environment.NewLine +
				"  But was:  <System.String>" + Environment.NewLine;
			Expect( "Hello", TypeOf( typeof(System.Int32) ) );
		}

		[Test]
		public void IsInstanceOfType()
		{
			Assert.IsInstanceOfType(typeof(System.Exception), new ApplicationException() );
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsInstanceOfTypeFails()
		{
			expectedMessage =
				"  Expected: instance of <System.Int32>" + System.Environment.NewLine + 
				"  But was:  <System.String>" + System.Environment.NewLine;
			Expect( "abc123", InstanceOfType( typeof(System.Int32) ) );
		}

		[Test]
		public void IsNotInstanceOfType()
		{
			Assert.IsNotInstanceOfType(typeof(System.Int32), "abc123" );
			Expect( "abc123", Not.InstanceOfType(typeof(System.Int32)) );
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsNotInstanceOfTypeFails()
		{
			expectedMessage =
				"  Expected: not instance of <System.Exception>" + System.Environment.NewLine + 
				"  But was:  <System.ApplicationException>" + System.Environment.NewLine;
			Assert.IsNotInstanceOfType( typeof(System.Exception), new ApplicationException() );
		}

		[Test()]
		public void IsAssignableFrom()
		{
			int [] array10 = new int [10];
			int [] array2 = new int[2];

			Assert.IsAssignableFrom(array2.GetType(),array10);
			Assert.IsAssignableFrom(array2.GetType(),array10,"Type Failure Message");
			Assert.IsAssignableFrom(array2.GetType(),array10,"Type Failure Message",null);
			Expect( array10, AssignableFrom( array2.GetType() ) );
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsAssignableFromFails()
		{
			int [] array10 = new int [10];
			int [,] array2 = new int[2,2];

			expectedMessage =
				"  Expected: Type assignable from <System.Int32[,]>" + System.Environment.NewLine + 
				"  But was:  <System.Int32[]>" + System.Environment.NewLine;
			Expect( array10, AssignableFrom( array2.GetType() ) );
		}

		[Test()]
		public void IsNotAssignableFrom()
		{
			int [] array10 = new int [10];
			int [,] array2 = new int[2,2];

			Assert.IsNotAssignableFrom(array2.GetType(),array10);
			Assert.IsNotAssignableFrom(array2.GetType(),array10,"Type Failure Message");
			Assert.IsNotAssignableFrom(array2.GetType(),array10,"Type Failure Message",null);
			Expect( array10, Not.AssignableFrom( array2.GetType() ) );
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsNotAssignableFromFails()
		{
			int [] array10 = new int [10];
			int [] array2 = new int[2];

			expectedMessage =
				"  Expected: not Type assignable from <System.Int32[]>" + System.Environment.NewLine + 
				"  But was:  <System.Int32[]>" + System.Environment.NewLine;
			Expect( array10, Not.AssignableFrom( array2.GetType() ) );
		}
	}
}
