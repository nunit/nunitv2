//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;

	/// <summary>
	/// 
	/// </summary>
	public interface ResultVisitor
	{
		void visit(TestCaseResult caseResult);
		void visit(TestSuiteResult suiteResult);
	}
}
