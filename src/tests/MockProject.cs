using System;
using NUnit.Util;

namespace NUnit.Tests
{
	public class MockProject : IProject
	{
		private bool isDirty = false;

		public bool IsDirty
		{
			get { return isDirty; }
			set { isDirty = value; }
		}
	}
}
