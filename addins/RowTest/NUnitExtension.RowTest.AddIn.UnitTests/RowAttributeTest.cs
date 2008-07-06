// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using NUnitExtension.RowTest;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class RowAttributeTest
	{
		[Test]
		public void ArgumentsAreProvided()
		{
			object[] arguments = new object[] { 4, 5, 6 };
			
			RowAttribute row = new RowAttribute(arguments);
			
			Assert.That(row.Arguments, Is.SameAs(arguments));
		}
		
		[Test]
		public void NullWasPassedAsArguments()
		{
			object[] arguments = null;
			
			RowAttribute row = new RowAttribute(arguments);
			
			Assert.That(row.Arguments, Is.Not.Null);
			Assert.That(row.Arguments.Length, Is.EqualTo(1));
			Assert.That(row.Arguments[0], Is.Null);
		}
		
		[Test]
		public void WithoutArguments()
		{
			RowAttribute row = new RowAttribute();
			
			Assert.That(row.Arguments, Is.Not.Null);
			Assert.That(row.Arguments, Is.Empty);
		}
	}
}
