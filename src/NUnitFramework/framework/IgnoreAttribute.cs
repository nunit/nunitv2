//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Framework
{
	using System;
	/// <summary>
	/// IgnoreAttribute.
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple=false)]
	public sealed class IgnoreAttribute : Attribute
	{
		private string reason;

		public IgnoreAttribute(string reason)
		{
			this.reason = reason;
		}

		public string Reason
		{
			get { return reason; }
		}
	}
}
