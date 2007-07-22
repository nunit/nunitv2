// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework.SyntaxHelpers;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class NotTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new NotConstraint( new EqualConstraint(null) );
            GoodValues = new object[] { 42, "Hello" };
            BadValues = new object [] { null };
            Description = "not null";
        }

		[Test,ExpectedException(typeof(AssertionException),ExpectedMessage="ignoring case",MatchType=MessageMatch.Contains)]
		public void NotHonorsIgnoreCaseUsingConstructors()
		{
			Assert.That( "abc", new NotConstraint( new EqualConstraint( "ABC" ).IgnoreCase ) );
		}

		[Test,ExpectedException(typeof(AssertionException),ExpectedMessage="ignoring case",MatchType=MessageMatch.Contains)]
		public void NotHonorsIgnoreCaseUsingPrefixNotation()
		{
			Assert.That( "abc", Is.Not.EqualTo( "ABC" ).IgnoreCase );
		}

		[Test,ExpectedException(typeof(AssertionException),ExpectedMessage="+/-",MatchType=MessageMatch.Contains)]
		public void NotHonorsTolerance()
		{
			Assert.That( 4.99d, Is.Not.EqualTo( 5.0d ).Within( .05d ) );
		}

        [Test]
        public void CanUseNotOperator()
        {
            Assert.That(42, !new EqualConstraint(99));
        }
    }
}
