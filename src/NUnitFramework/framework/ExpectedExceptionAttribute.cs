//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Framework
{
	using System;

	/// <summary>
	/// ExpectedAttributeException.
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public sealed class ExpectedExceptionAttribute : Attribute
	{
		private Type expectedException;

		public ExpectedExceptionAttribute(Type exceptionType)
		{
			expectedException = exceptionType;
		}

		public Type ExceptionType 
		{
			get{ return expectedException; }
			set{ expectedException = value; }
		}

}
}
