//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Framework
{
	using System;
	/// <summary>
	/// TestAttribute.
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public sealed class TestAttribute : Attribute
	{}
}
