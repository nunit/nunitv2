using System;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for ProjectConfigCollection.
	/// </summary>
	public class ProjectConfigCollection : CollectionBase
	{
		private IProject project;

		public ProjectConfigCollection( IProject project ) 
		{ 
			this.project = project;
		}

		#region Properties

		public IProject Project
		{
			get { return project; }
		}

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
			List.Add( config );
			config.Project = project;
		}

		public void Add( string name )
		{
			Add( new ProjectConfig( name ) );
		}

		public void Remove( ProjectConfig config )
		{
			List.Remove( config );
		}

		public void Remove( string name )
		{
			int index = IndexOf( name );
			if ( index >= 0 )
			{
				RemoveAt( index );
			}
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

		protected override void OnRemoveComplete( int index, object obj )
		{
			project.IsDirty = true;
		}

		protected override void OnInsertComplete( int index, object obj )
		{
			project.IsDirty = true;
		}

		protected override void OnSetComplete( int index, object oldValue, object newValue )
		{
			project.IsDirty = true;
		}

		#endregion
	}
}
