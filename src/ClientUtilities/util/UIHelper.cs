//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
//
namespace NUnit.Util
{
	using System;
	using NUnit.Core;

	/// <summary>
	/// Summary description for UIHelper.
	/// </summary>
	public class UIHelper
	{
		private static bool AreNodesTheSame(Test testOne, Test testTwo)
		{
			if(testOne==null && testTwo!=null) return false;
			if(testTwo==null && testOne!=null) return false;
			if(testOne.GetType().FullName != testTwo.GetType().FullName) return false;
			return testOne.FullName.Equals(testTwo.FullName);
		}

		public static bool CompareTree(Test rootTestOriginal, Test rootTestNew)
		{
			if(!AreNodesTheSame(rootTestOriginal,rootTestNew)) return false;
			if((rootTestOriginal is TestSuite) && (rootTestNew is TestSuite))
			{
				TestSuite originalSuite = (TestSuite)rootTestOriginal;
				TestSuite newSuite = (TestSuite)rootTestNew;
				int originalCount = originalSuite.Tests.Count;
				int newCount = newSuite.Tests.Count;
				if(originalCount!=newCount)
				{
					return false;
				}
				for(int i=0; i<originalSuite.Tests.Count;i++)
				{
					if(!CompareTree((Test)originalSuite.Tests[i],(Test)newSuite.Tests[i])) return false;
				}
			}
			return true;
		}
	}
}
