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

		public string this[int index]
		{
			get { return menu.MenuItems[index].Text.Substring( 2 ); }
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

//			if ( AppUI.TestLoader.IsProjectLoaded )
//				AppUI.TestLoaderUI.CloseProject();
//
//			AppUI.TestLoader.LoadTest( testFileName );

			AppUI.TestLoaderUI.OpenProject( testFileName );
		}
	}
}
