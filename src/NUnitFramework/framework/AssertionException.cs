//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Framework 
{
	using System;
	using System.Runtime.Serialization;
	
	/// <summary>
	/// Thrown when an assertion failed.
	/// </summary>
	/// 
	[Serializable]
	public class AssertionException : ApplicationException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public AssertionException (string message) : base(message) 
		{}

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		/// <param name="inner">The exception that caused the 
		/// current exception</param>
		public AssertionException(string message, Exception inner) :
			base(message, inner) 
		{}

		/// <summary>
		/// Serialization Constructor
		/// </summary>
		protected AssertionException(SerializationInfo info, 
			StreamingContext context) : base(info,context)
		{}

	}
}
