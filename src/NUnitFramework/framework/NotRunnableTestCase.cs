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
using System;
using System.Reflection;
using System.Diagnostics;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for NotRunnableTestCase.
	/// </summary>
	public class NotRunnableTestCase : TestCase
	{
		public NotRunnableTestCase(MethodInfo method, string reason) : base(method.DeclaringType.FullName, method.Name)
		{
			ShouldRun = false;
			IgnoreReason = reason;
		}

		public NotRunnableTestCase(MethodInfo method) : base(method.DeclaringType.FullName, method.Name)
		{
			string reason;

			if (method.IsAbstract)
				reason = "it must not be abstract";
			else if (!method.IsPublic)
				reason = "it must be a public method";
			else if (method.GetParameters().Length != 0)
				reason = "it must not have parameters";
			else if (!method.ReturnType.Equals(typeof(void)))
				reason = "it must return void";
			else
				reason = "reason not known";

			ShouldRun = false;
			IgnoreReason = String.Format("Method {0}'s signature is not correct: {1}.", method.Name, reason);
		}

		public override void Run(TestCaseResult result)
		{
			result.NotRun(base.IgnoreReason);
		}
	}
}

