using System;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;

namespace NUnit.Tests.Core
{
	/// <summary>
	/// Summary description for ILogicalThreadAffinativeTests.
	/// </summary>
	[TestFixture]
	public class ILogicalThreadAffinativeTests
	{
		[TearDown]
		public void FreeCallContext()
		{
			//This is really just a workaround. Tests must free any context
			//data slots that have been allocated to avoid serialization problems
			CallContext.FreeNamedDataSlot("MyContextData");
		}

		[Test]
		public void DatabaseAndMemoryQueryTest()
		{		
			LogicalCallContextData lcd = new LogicalCallContextData();
			lcd.SetData("hello there!");
			Console.WriteLine(lcd.GetData());
		}
	}

	/// <summary>
	/// Helper class that implements ILogicalThreadAffinative
	/// </summary>
	[Serializable]
	public class LogicalCallContextData : ILogicalThreadAffinative
	{
		const string CONTEXT_DATA = "MyContextData";
		private string _data = "<empty>";

		public LogicalCallContextData()
		{			
		}

		public void SetData(string data)
		{
			_data = data;
			CallContext.SetData(CONTEXT_DATA, this);
		}

		public string GetData()
		{
			LogicalCallContextData data = (LogicalCallContextData)CallContext.GetData(CONTEXT_DATA);
			return data._data;
		}
		
	}
}
