using System;

namespace Nunit.Core
{
	/// <summary>
	/// Summary description for InvalidFixture.
	/// </summary>
	public class InvalidFixture
	{
		private Type fixtureType;
		private string message;

		public InvalidFixture(Type original, string why)
		{
			fixtureType = original;
			message = why;
		}

		public Type OriginalType
		{
			get { return fixtureType; }
		}

		public string Message
		{
			get { return message; }
		}
	}
}
