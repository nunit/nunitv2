//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;
	using System.IO;

	/// <summary>
	/// Summary description for StackTraceFilter.
	/// </summary>
	public class StackTraceFilter
	{
		public static string Filter(string stack) 
		{
			if(stack == null) return null;
			StringWriter sw = new StringWriter();
			StringReader sr = new StringReader(stack);

			try 
			{
				string line;
				while ((line = sr.ReadLine()) != null) 
				{
					if (!FilterLine(line))
						sw.WriteLine(line);
				}
			} 
			catch (Exception) 
			{
				return stack;
			}
			return sw.ToString();
		}

		static bool FilterLine(string line) 
		{
			string[] patterns = new string[]
			{
				"NUnit.Core.TestCase",
				"NUnit.Core.ExpectedExceptionTestCase",
				"NUnit.Core.TemplateTestCase",
				"NUnit.Core.TestResult",
				"NUnit.Core.TestSuite",
				"NUnit.Framework.Assertion" 
			};

			for (int i = 0; i < patterns.Length; i++) 
			{
				if (line.IndexOf(patterns[i]) > 0)
					return true;
			}

			return false;
		}

	}
}
