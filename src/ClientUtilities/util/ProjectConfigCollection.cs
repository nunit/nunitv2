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

		public void Add( ProjectConfig config )
		{
			InnerList.Add( config );
		}

		public void Add( string name )
		{
			InnerList.Add( new ProjectConfig( name ) );
		}

		public void Remove( ProjectConfig config )
		{
			InnerList.Remove( config );
		}

		public void Remove( string name )
		{
			foreach( ProjectConfig config in InnerList )
				if ( config.Name == name )
				{
					Remove( config );
					break;
				}
		}

		public bool Contains( string name )
		{
			foreach( ProjectConfig config in InnerList )
				if ( config.Name == name )
					return true;
			
			return false;
		}

		public ProjectConfig this[string name]
		{
			get 
			{ 
				foreach ( ProjectConfig config in InnerList )
					if ( config.Name == name )
						return config;

				return null;
			}
		}
	}
}
