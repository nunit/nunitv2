//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Summary description for NoTestMethodsException.
	/// </summary>
	/// 
	[Serializable]
	public class InvalidTestFixtureException : ApplicationException
	{
		public InvalidTestFixtureException() : base() {}

		public InvalidTestFixtureException(string message) : base(message)
		{}

		public InvalidTestFixtureException(string message, Exception inner) : base(message, inner)
		{}

		/// <summary>
		/// Serialization Constructor
		/// </summary>
		protected InvalidTestFixtureException(SerializationInfo info, 
			StreamingContext context) : base(info,context){}

	}
}