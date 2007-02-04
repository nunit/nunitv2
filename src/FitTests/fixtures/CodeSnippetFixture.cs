using System;
using System.Reflection;
using System.CodeDom.Compiler;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Fixtures
{
	/// <summary>
	/// Abstract base class for fixtures that compile a snippet of code.
	/// The fixture is basically a column fixture with one input column
	/// dedicated to containing the code that is to be compiled. This
	/// will normally be the first column
	/// </summary>
	public class CodeSnippetFixture : fit.ColumnFixture
	{
		[Flags]
		public enum Action
		{
			Compile,
			Load,
			Run
		}

		public string Code;

		private static readonly string testAssembly = "test.dll";
		private Action action;

		protected TestRunner testRunner;
		protected TestResult testResult;
		protected ResultSummarizer testSummary;

		public CodeSnippetFixture( Action action )
		{
			this.action = action;
		}

		// Override doCell to handle the 'Code' column. We compile
		// the code and optionally load and run the tests.
		public override void doCell(fit.Parse cell, int columnNumber)
		{
			base.doCell (cell, columnNumber);

			FieldInfo field = columnBindings[columnNumber].field;
			if ( field != null && field.Name == "Code" )
				ProcessCodeSnippet( cell, Code );
		}

		protected virtual void ProcessCodeSnippet( fit.Parse cell, string code )
		{
			if ( CompileCodeSnippet( cell, code ) )
			{
				if ( action == Action.Load || action == Action.Run )
				{
					testRunner = new TestDomain();
					if ( !testRunner.Load( new TestPackage(testAssembly) ) )
					{
						this.wrong(cell);
						cell.addToBody( string.Format( 
							"<font size=-1 color=\"#c08080\"> <i>Failed to load {0}</i></font>", testAssembly ) );

						return;
					}
				}

				if ( action == Action.Run )
				{
					testResult = testRunner.Run(NullListener.NULL);
					testSummary = new ResultSummarizer( testResult );
				}

				this.right( cell );
			}
		}

		protected virtual bool CompileCodeSnippet( fit.Parse cell, string code )
		{
			CompilerResults results = CompileCode( code );
			if ( results.NativeCompilerReturnValue == 0 )
				return true;

			cell.addToBody( "<font size=-1 color=\"#c08080\"><i>Compiler errors</i></font>" );

			wrong( cell );
			cell.addToBody( "<hr>" );
				
			foreach( string line in results.Output )
				cell.addToBody( line + "<br>" );

			return false;
		}

		public override void wrong(fit.Parse cell)
		{
			string body = cell.body;
			base.wrong (cell);
			cell.body = body;
		}

		private CompilerResults CompileCode( string code )
		{
			Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
			ICodeCompiler compiler = provider.CreateCompiler();

			CompilerParameters options = new CompilerParameters();
			options.ReferencedAssemblies.Add( "system.dll" );
			options.ReferencedAssemblies.Add( "nunit.framework.dll" );
			options.IncludeDebugInformation = false;
			options.TempFiles = new TempFileCollection( ".", false );
			options.OutputAssembly = testAssembly;
			options.GenerateInMemory = false;

			return compiler.CompileAssemblyFromSource( options, code );
		}
	}
}
