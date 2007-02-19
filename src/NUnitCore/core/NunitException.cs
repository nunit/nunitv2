// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Core 
{
	using System;
	using System.Runtime.Serialization;
  
	/// <summary>
	/// Thrown when an assertion failed. Here to preserve the inner
	/// exception and hence its stack trace.
	/// </summary>
	/// 
	[Serializable]
	public class NunitException : ApplicationException 
	{
		public NunitException () : base() 
		{} 

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		public NunitException(string message) : base (message)
		{}

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		/// <param name="inner">The exception that caused the 
		/// current exception</param>
		public NunitException(string message, Exception inner) :
			base(message, inner) 
		{}

		/// <summary>
		/// Serialization Constructor
		/// </summary>
		protected NunitException(SerializationInfo info, 
			StreamingContext context) : base(info,context){}


	}
}
