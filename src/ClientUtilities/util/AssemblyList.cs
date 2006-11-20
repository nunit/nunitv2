#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.IO;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Represents a list of assemblies. It stores paths 
	/// that are added and fires an event whenevever it
	/// changes. All paths must be added as absolute paths.
	/// </summary>
	public class AssemblyList : CollectionBase
	{
		#region Properties and Events
		public string this[int index]
		{
			get { return (string)List[index]; }
			set 
			{ 
				if ( !Path.IsPathRooted( value ) )
					throw new ArgumentException( "Assembly path must be absolute" );
				List[index] = value; 
			}
		}

		public event EventHandler Changed;
		#endregion

		#region Methods
		public string[] ToArray()
		{
			return (string[])InnerList.ToArray( typeof( string ) );
		}

		public void Add( string assemblyPath )
		{
			if ( !Path.IsPathRooted( assemblyPath ) )
				throw new ArgumentException( "Assembly path must be absolute" );
			List.Add( assemblyPath );
		}

		public void Remove( string assemblyPath )
		{
			for( int index = 0; index < this.Count; index++ )
			{
				if ( this[index] == assemblyPath )
					RemoveAt( index );
			}
		}

		protected override void OnRemoveComplete(int index, object value)
		{
			FireChangedEvent();
		}

		protected override void OnInsertComplete(int index, object value)
		{
			FireChangedEvent();
		}

		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			FireChangedEvent();
		}

		private void FireChangedEvent()
		{
			if ( Changed != null )
				Changed( this, EventArgs.Empty );
		}
		#endregion
	}
}
