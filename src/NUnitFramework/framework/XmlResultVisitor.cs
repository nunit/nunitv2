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

		public XmlResultVisitor(string fileName, TestResult result)
		{
			ResultSummarizer summaryResults = new ResultSummarizer(result);
			try
			{
				xmlWriter = new XmlTextWriter (fileName, null);
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

		public void visit(TestCaseResult caseResult) 
		{
			xmlWriter.WriteStartElement("test-case");
			xmlWriter.WriteAttributeString("name",caseResult.Name);
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

		public void visit(TestSuiteResult suiteResult) 
		{
			xmlWriter.WriteStartElement("test-suite");
			xmlWriter.WriteAttributeString("name",suiteResult.Name);
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
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndDocument();
			xmlWriter.Flush();
			xmlWriter.Close();
		}
	}
}
