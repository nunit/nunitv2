/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
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