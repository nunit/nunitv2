namespace NUnit.Core
{
	using System;

	[Serializable]
	public class TestOutput
	{
		string text;
		TestOutputType type;

		public TestOutput(string text, TestOutputType type)
		{
			this.text = text;
			this.type = type;
		}

		public override string ToString()
		{
			return type + ": " + text;
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public TestOutputType Type
		{
			get
			{
				return this.type;
			}
		}
	}

	public enum TestOutputType
	{
		Out, Error
	}
}
