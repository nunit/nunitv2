// ***********************************************************************
// Copyright (c) 2010 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;

namespace NUnit.ProjectEditor
{
	/// <summary>
	/// Exception raised when loading a project file with
	/// an invalid format.
	/// </summary>
	public class ProjectFormatException : ApplicationException
	{
		#region Instance Variables

		private int lineNumber;

		private int linePosition;

		#endregion

		#region Constructors

		public ProjectFormatException() : base() {}

		public ProjectFormatException( string message )
			: base( message ) {}

		public ProjectFormatException( string message, Exception inner )
			: base( message, inner ) {}

		public ProjectFormatException( string message, int lineNumber, int linePosition )
			: base( message )
		{
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		#endregion

		#region Properties

		public int LineNumber
		{
			get { return lineNumber; }
		}

		public int LinePosition
		{
			get { return linePosition; }
		}

		#endregion
	}
}
