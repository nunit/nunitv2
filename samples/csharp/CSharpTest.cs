//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Samples 
{
	using System;
	using NUnit.Framework;

	/// <summary>Some simple Tests.</summary>
	/// 
	[TestFixture] 
	public class SimpleCSharpTest
	{
		/// <summary>
		/// 
		/// </summary>
		protected int fValue1;
		/// <summary>
		/// 
		/// </summary>
		protected int fValue2;
		
		/// <summary>
		/// 
		/// </summary>
		[SetUp] public void Init() 
		{
			fValue1= 2;
			fValue2= 3;
		}

		/// <summary>
		/// 
		/// </summary>
		///
		[Test] public void Add() 
		{
			double result= fValue1 + fValue2;
			// forced failure result == 5
			Assertion.AssertEquals("Expected Failure.",6,result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// 
		[Test] public void DivideByZero() 
		{
			int zero= 0;
			int result= 8/zero;
		}

		/// <summary>
		/// 
		/// </summary>
		/// 
		[Test] public void Equals() 
		{
			Assertion.AssertEquals("Integer.",12, 12);
			Assertion.AssertEquals("Long.",12L, 12L);
			Assertion.AssertEquals("Char.",'a', 'a');
			Assertion.AssertEquals("Integer Object Cast.",(object)12, (object)12);
            
			Assertion.AssertEquals("Expected Failure (Integer).", 12, 13);
			Assertion.AssertEquals("Expected Failure (Double).", 12.0, 11.99, 0.0);
		}
	}
}