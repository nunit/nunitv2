namespace NUnit.Core.Tests
{
	using System;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Reflection;
	using System.Diagnostics;
	using System.Collections;
	using System.CodeDom.Compiler;
	using System.Runtime.Remoting.Messaging;
	using Microsoft.CSharp;
	using NUnit.Core;
	using NUnit.Framework;

	[TestFixture]
	public class ThreadedTestRunnerTests
	{
		string outputName;
		TestRunner testRunner;
		const string CONTEXT_DATA_NAME = "__CONTEXT_DATA_NAME__";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			CSharpCodeProvider provider = new CSharpCodeProvider();
			ICodeCompiler compiler = provider.CreateCompiler();
			string[] assemblyNames = new string[]
			{
				typeof(TestAttribute).Assembly.Location
			};
			this.outputName = "ThreadedTestRunnerTests.dll";
			CompilerParameters options = new CompilerParameters(assemblyNames, this.outputName);
			string source = 
@"
using System;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;

[TestFixture]
public class MyFixture
{
	[Test] public void MyTest()
	{
		Console.WriteLine(""Hello, World!"");
	}

	[Test] public void MyTestSetData()
	{
		CallContext.SetData( ""$(CONTEXT_DATA_NAME)"", new EmptyCallContextData() );
		Console.WriteLine(""Hello, World!"");
		Console.Error.WriteLine(""Hello, World!"");
	}

	[Serializable]
	public class EmptyCallContextData : ILogicalThreadAffinative {}
}
";
			source = source.Replace("$(CONTEXT_DATA_NAME)", CONTEXT_DATA_NAME);
			CompilerResults results = compiler.CompileAssemblyFromSource(options, source);
			foreach(object error in results.Errors)
			{
				Debug.WriteLine(error);
			}
			Assert.AreEqual(0, results.NativeCompilerReturnValue);
		}

		[SetUp]
		public void SetUp()
		{
			TestRunner testRunner = new RealTestRunner();
			this.testRunner = new ThreadedTestRunner(testRunner);
			this.testRunner.Load(this.outputName);
		}

		[TearDown]
		public void TearDown()
		{
			this.testRunner.Unload();
		}

		[Test]
		public void SetDataEventListenerTest()
		{
			string[] testNames =
			{
				"MyFixture.MyTestSetData"
			};
			CallContextDataEventListener listener = new CallContextDataEventListener(CONTEXT_DATA_NAME);

			RunWorkItem workItem = new RunWorkItem(this.testRunner, listener, testNames);
			Thread thread = new Thread(new ThreadStart(workItem.Run));
			thread.Start();
			thread.Join();

			Assert.IsNull(listener.Data, "Check that EventListener thread doesn't contain CallContext data");
			Assert.IsTrue(listener.EventCount > 0, "Check that EventListener was called at least once");
		}

		class RunWorkItem
		{
			TestRunner testRunner;
			EventListener listener;
			string[] testNames;

			public RunWorkItem(TestRunner testRunner, EventListener listener, string[] testNames)
			{
				this.testRunner = testRunner;
				this.listener = listener;
				this.testNames = testNames;
			}

			public void Run()
			{
				this.testRunner.Run(listener, testNames);
			}
		}

		class CallContextDataEventListener : MarshalByRefObject, EventListener
		{
			string name;
			object data;
			int eventCount;

			public CallContextDataEventListener(string name)
			{
				this.name = name;
			}

			public void TestStarted(NUnit.Core.TestCase testCase)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void RunStarted(Test[] tests)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void RunFinished(Exception exception)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void RunFinished(TestResult[] results)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void SuiteFinished(TestSuiteResult result)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void TestFinished(TestCaseResult result)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void TestOutput(TestOutput testOutput)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void UnhandledException(Exception exception)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public void SuiteStarted(TestSuite suite)
			{
				this.data = CallContext.GetData(this.name);
				this.eventCount++;
			}

			public int EventCount
			{
				get
				{
					return this.eventCount;
				}
			}

			public object Data
			{
				get
				{
					return this.data;
				}
			}
		}
	}
}
