using System;

namespace NUnit.Framework.Extensions
{
	/// <summary>
	/// Summary description for MaxTimeAttribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple=false, Inherited=false )]
	public sealed class MaxTimeAttribute : Attribute
	{
		private int maxTime;

		public MaxTimeAttribute( int maxTime )
		{
			this.maxTime = maxTime;
		}

		public int MaxTime
		{
			get { return maxTime; }
		}
	}
}
