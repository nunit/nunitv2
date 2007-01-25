using System;

namespace NUnit.Framework.Extensions
{
	/// <summary>
	/// Summary description for ExpectFailureAttribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple=false, Inherited=false )]
	public sealed class ExpectFailureAttribute : Attribute
	{
	}
}
