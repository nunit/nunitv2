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
namespace NUnit.Core
{
	using System;
	using System.Reflection;

	/// <summary>
	/// Summary description for TestCase.
	/// </summary>
	public class NormalTestCase : TemplateTestCase
	{
		public NormalTestCase(object fixture, MethodInfo method) : base(fixture, method)
		{}

		protected internal override void ProcessNoException(TestCaseResult testResult)
		{
			testResult.Success();
		}
		
		protected internal override void ProcessException(Exception exception, TestCaseResult testResult)
		{
			if(exception.GetType().IsAssignableFrom(typeof(NUnit.Framework.AssertionException)))
			{
				NUnit.Framework.AssertionException error = (NUnit.Framework.AssertionException)exception;
				testResult.Failure(error.Message, error.StackTrace);
			}
			else
			{
				testResult.Failure(exception.Message, exception.StackTrace);
			}
		}
	}
}

