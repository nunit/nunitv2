#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Globalization;
	using System.IO;
	using System.Xml;
	using NUnit.Core;

	/// <summary>
	/// Summary description for XmlResultVisitor.
	/// </summary>
	public class XmlResultVisitor : ResultVisitor
	{
		private XmlTextWriter xmlWriter;
		private TextWriter writer;

		public XmlResultVisitor(string fileName, TestResult result)
		{
			writer = new StreamWriter(fileName, false, System.Text.Encoding.Unicode);
			Initialize(result);
		}

		public XmlResultVisitor(TextWriter writer, TestResult result) 
		{
			this.writer = writer;
			Initialize(result);
		}

		private void Initialize(TestResult result) 
		{
			ResultSummarizer summaryResults = new ResultSummarizer(result);
			try
			{
				xmlWriter = new XmlTextWriter (writer);
			}
			catch(Exception e)
			{
				Console.Error.WriteLine(e.StackTrace);
			}

			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.WriteStartDocument(false);
			xmlWriter.WriteComment("This file represents the results of running a test suite");

			xmlWriter.WriteStartElement("test-results");

			xmlWriter.WriteAttributeString("name", summaryResults.Name);
			xmlWriter.WriteAttributeString("total", summaryResults.ResultCount.ToString());
			xmlWriter.WriteAttributeString("failures", summaryResults.Failures.ToString());
			xmlWriter.WriteAttributeString("not-run", summaryResults.TestsNotRun.ToString());

			DateTime now = DateTime.Now;
			xmlWriter.WriteAttributeString("date", now.ToShortDateString());
			xmlWriter.WriteAttributeString("time", now.ToShortTimeString());
		}

		public void Visit(TestCaseResult caseResult) 
		{
			xmlWriter.WriteStartElement("test-case");
			xmlWriter.WriteAttributeString("name",caseResult.Name);

			if(caseResult.Description != null)
				xmlWriter.WriteAttributeString("description", caseResult.Description);

			xmlWriter.WriteAttributeString("executed", caseResult.Executed.ToString());
			if(caseResult.Executed)
			{
				xmlWriter.WriteAttributeString("success", caseResult.IsSuccess.ToString());

				xmlWriter.WriteAttributeString("time", caseResult.Time.ToString("#####0.000", NumberFormatInfo.InvariantInfo));

				if(caseResult.IsFailure)
				{
					if(caseResult.IsFailure)
						xmlWriter.WriteStartElement("failure");
					else
						xmlWriter.WriteStartElement("error");
				
					xmlWriter.WriteStartElement("message");
					xmlWriter.WriteCData(caseResult.Message);
					xmlWriter.WriteEndElement();
				
					xmlWriter.WriteStartElement("stack-trace");
					if(caseResult.StackTrace != null)
						xmlWriter.WriteCData(StackTraceFilter.Filter(caseResult.StackTrace));
					xmlWriter.WriteEndElement();
				
					xmlWriter.WriteEndElement();
				}
				
			}
			else
			{
				xmlWriter.WriteStartElement("reason");
				xmlWriter.WriteStartElement("message");
				xmlWriter.WriteCData(caseResult.Message);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
            
			xmlWriter.WriteEndElement();
		}

		public void Visit(TestSuiteResult suiteResult) 
		{
			xmlWriter.WriteStartElement("test-suite");
			xmlWriter.WriteAttributeString("name",suiteResult.Name);
			if(suiteResult.Description != null)
				xmlWriter.WriteAttributeString("description", suiteResult.Description);

			xmlWriter.WriteAttributeString("success", suiteResult.IsSuccess.ToString());
			xmlWriter.WriteAttributeString("time", suiteResult.Time.ToString());
            
			xmlWriter.WriteStartElement("results");                  
			foreach (TestResult result in suiteResult.Results)
			{
				result.Accept(this);
			}
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();
		}

		public void Write()
		{
			try 
			{
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
				xmlWriter.Flush();
				xmlWriter.Close();
			} 
			finally 
			{
				//writer.Close();
			}
		}
	}
}
