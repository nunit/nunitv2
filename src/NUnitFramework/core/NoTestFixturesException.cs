//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Summary description for NoTestFixtureException.
	/// </summary>
	[Serializable]
	public class NoTestFixturesException : ApplicationException
	{
		public NoTestFixturesException() : base () {}

		public NoTestFixturesException(string message) : base(message)
		{}

		public NoTestFixturesException(string message, Exception inner) : base(message, inner) {}

		protected NoTestFixturesException(SerializationInfo info, StreamingContext context) : base(info, context)
		{}
	}
}
