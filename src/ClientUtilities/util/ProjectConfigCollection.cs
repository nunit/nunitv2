using System;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for ProjectConfigCollection.
	/// </summary>
	public class ProjectConfigCollection : CollectionBase
	{
		public ProjectConfigCollection() { }

		#region Properties

		public ArrayList Names
		{
			get
			{
				ArrayList names = new ArrayList();
				
				foreach( ProjectConfig config in InnerList )
					names.Add( config.Name );

				return names;
			}
		}

		public ProjectConfig this[int index]
		{
			get { return (ProjectConfig)InnerList[index]; }
		}

		public ProjectConfig this[string name]
		{
			get 
			{ 
				int index = IndexOf( name );
				return index >= 0 ? (ProjectConfig)InnerList[index]: null;
			}
		}

		#endregion

		#region Methods

		public void Add( ProjectConfig config )
		{
			InnerList.Add( config );
		}

		public void Add( string name )
		{
			Add( new ProjectConfig( name ) );
		}

		public void CopyTo( ProjectConfig[] array )
		{
			InnerList.CopyTo( array );
		}

		public void Remove( ProjectConfig config )
		{
			InnerList.Remove( config );
		}

		public void Remove( string name )
		{
			int index = IndexOf( name );
			if ( index >= 0 )
				RemoveAt( index );
		}

		public int IndexOf( ProjectConfig config )
		{
			return InnerList.IndexOf( config );
		}

		public int IndexOf( string name )
		{
			for( int index = 0; index < InnerList.Count; index++ )
			{
				ProjectConfig config = (ProjectConfig)InnerList[index];
				if( config.Name == name )
					return index;
			}

			return -1;
		}

		public bool Contains( ProjectConfig config )
		{
			return InnerList.Contains( config );
		}

		public bool Contains( string name )
		{
			return IndexOf( name ) >= 0;
		}

		protected override void OnInsert( int index, object obj )
		{
			ProjectConfig config = (ProjectConfig)obj;

			if ( this.Contains( config.Name ) )
				throw new ArgumentException( "Collection already contains a configuration with this name" );		
		}

		private void OnConfigChanged( ProjectConfig config )
		{
		}

		#endregion
	}
}
