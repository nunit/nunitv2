using System;
using System.Collections;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for RecentFileMenu.
	/// </summary>
	public class RecentFileMenuHandler
	{
		private MenuItem menu;
		private RecentFileSettings recentFiles;

		public RecentFileMenuHandler( MenuItem menu, RecentFileSettings recentFiles )
		{
			this.menu = menu;
			this.recentFiles = recentFiles;
		}

		public MenuItem Menu
		{
			get { return menu; }
		}

		public void Load()
		{
			IList files = recentFiles.GetFiles();

			if ( files.Count == 0 )
				Menu.Enabled = false;
			else 
			{
				Menu.Enabled = true;
				Menu.MenuItems.Clear();
				int index = 1;
				foreach ( string name in files ) 
				{
					MenuItem item = new MenuItem(String.Format("{0} {1}", index++, name));
					item.Click += new System.EventHandler( OnRecentFileClick );
					Menu.MenuItems.Add( item );
				}		
			}
		}

		private void OnRecentFileClick( object sender, EventArgs e )
		{
			MenuItem item = (MenuItem) sender;
			string testFileName = item.Text.Substring( 2 );

			AppUI.TestLoader.LoadTest( testFileName );
		}
	}
}
