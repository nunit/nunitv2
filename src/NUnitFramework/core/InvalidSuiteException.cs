//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Core
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class InvalidSuiteException : ApplicationException 
	{
		public InvalidSuiteException () : base() 
		{} 

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		public InvalidSuiteException(string message) : base (message)
		{}

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		/// <param name="inner">The exception that caused the 
		/// current exception</param>
		public InvalidSuiteException(string message, Exception inner) :
			base(message, inner) 
		{}

		/// <summary>
		/// Serialization Constructor
		/// </summary>
		protected InvalidSuiteException(SerializationInfo info, 
			StreamingContext context) : base(info,context){}

	}
}