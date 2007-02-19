// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CP.Windows.Shell
{
	/// <summary>
	/// Class to wrap the Shell32 function SHBrowseForFolder
	/// allowing the user to select a folder. 
	/// </summary>
	public class FolderBrowser
	{
		private Form owner;

		private string caption = null;

		private string title = null;

		private string selection = null;

		private IntPtr pidlRoot = (IntPtr) null;

		/// <summary>
		/// Default constructor browses from My Computer
		/// </summary>
		public FolderBrowser( Form owner ) : this( owner, CSIDL_DRIVES ) { }

		/// <summary>
		/// Constructor to browse from a special folder location
		/// </summary>
		public FolderBrowser( Form owner, int nFolder )
		{
			this.owner = owner;

			SHGetSpecialFolderLocation( owner.Handle, nFolder, out pidlRoot );
		}

		/// <summary>
		/// Constructor to browse from a given FS path
		/// </summary>
		public FolderBrowser( Form owner, string rootPath )
		{
			this.owner = owner;

			IShellFolder pDesktop;
			SHGetDesktopFolder( out pDesktop );
			
			int chEaten = 0;
			int attrs = 0;
			pDesktop.ParseDisplayName( owner.Handle, (IntPtr)null, rootPath, out chEaten, ref pidlRoot, ref attrs );
		}

		/// <summary>
		/// Set/Get the caption text
		/// </summary>
		public string Caption
		{
			get { return caption; }
			set { caption = value; }
		}

		/// <summary>
		/// Although called a title in the SHBrowseForFolder
		/// documentation, this refers to the text displayed
		/// in the dialog below the caption bar.
		/// </summary>
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		public string InitialSelection
		{
			set { selection = value; }
		}

		/// <summary>
		/// It all comes down to this worker method which
		/// browses based on a pidl.
		/// </summary>
		public string BrowseForFolder()
		{
			BROWSEINFO bi = new BROWSEINFO();

			bi.hwndOwner = owner.Handle;
			bi.lpszTitle = title;
			bi.ulFlags = 
				BIF_NEWDIALOGSTYLE | 
				BIF_RETURNONLYFSDIRS | 
				BIF_DONTGOBELOWDOMAIN |
				BIF_STATUSTEXT;
			bi.pidlRoot = pidlRoot;
			bi.lpfn = new BrowseCallbackHandler( BrowseCallback );
		
			IntPtr pidl = SHBrowseForFolder( ref bi );
			if ( pidl == (IntPtr)0 ) return null;

			StringBuilder sb = new StringBuilder( MAX_PATH );
			SHGetPathFromIDList( pidl, sb );
			return sb.ToString();
		}

		private int BrowseCallback(
			IntPtr hwnd,
			uint msg,
			uint lParam,
			uint dwData )
		{
			switch ( msg )
			{
				case BFFM_INITIALIZED:
					if ( selection != null )
						SendMessage( hwnd, BFFM_SETSELECTION, 1, selection );
					if ( caption != null )
						SetWindowText( hwnd, caption );
					break;

				default:
					//MessageBox.Show( string.Format( "Got message {0}", msg ) );
					break;
			}

			return 0;
		}
		
		#region P/Invoke Stuff

		private const int CSIDL_DRIVES = 0x0011;
		private const int MAX_PATH = 256;

		private const uint BIF_RETURNONLYFSDIRS   =	0x0001;
		private const uint BIF_DONTGOBELOWDOMAIN  =	0x0002;
		private const uint BIF_STATUSTEXT         =	0x0004;
		private const uint BIF_RETURNFSANCESTORS  =	0x0008;
		private const uint BIF_EDITBOX            =	0x0010;
		private const uint BIF_VALIDATE           =	0x0020;
		private const uint BIF_NEWDIALOGSTYLE     =	0x0040;
		private const uint BIF_USENEWUI           =	(BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
		private const uint BIF_BROWSEINCLUDEURLS  =	0x0080;
		private const uint BIF_BROWSEFORCOMPUTER  =	0x1000;
		private const uint BIF_BROWSEFORPRINTER   =	0x2000;
		private const uint BIF_BROWSEINCLUDEFILES =	0x4000;
		private const uint BIF_SHAREABLE          =	0x8000;

		private const uint BFFM_INITIALIZED		  = 1;
		private const uint BFFM_SELCHANGED		  = 2;
		private const uint BFFM_VALIDATEFAILEDA	  = 3;
		private const uint BFFM_VALIDATEFAILEDW	  = 4;
		private const uint BFFM_IUNKNOWN		  = 5;

		private const uint WM_USER				  = 0x0400;
		private const uint BFFM_SETSELECTION	  = WM_USER + 103;

		[StructLayout(LayoutKind.Sequential)]
		private struct BROWSEINFO 
		{ 
			public IntPtr		hwndOwner; 
			public IntPtr		pidlRoot; 
			public IntPtr 		pszDisplayName;
			[MarshalAs(UnmanagedType.LPStr)] 
			public string 		lpszTitle; 
			public uint 		ulFlags; 
			public BrowseCallbackHandler lpfn; 
			public int			lParam; 
			public IntPtr 		iImage; 
		} 

		[DllImport("Shell32.dll")]
		private static extern IntPtr SHBrowseForFolder(
			ref BROWSEINFO lpbi );

		[DllImport("Shell32.dll")]
		private static extern uint SHGetPathFromIDList(
			IntPtr pidl,
			StringBuilder path );

		[DllImport("Shell32.Dll")]
		private static extern uint SHGetSpecialFolderLocation(
			IntPtr hwndOwner,
			int nFolder,
			out IntPtr pidl );

		[DllImport("Shell32.dll")]
		private static extern uint SHGetDesktopFolder(
			out IShellFolder pShellFolder );

		[DllImport("User32.dll")]
		private static extern uint SetWindowText( 
			IntPtr hwnd, 
			string text );

		[DllImport("User32.dll")]
		private static extern uint SendMessage(
			IntPtr hwnd,
			uint msg,
			int wParam,
			[MarshalAs( UnmanagedType.LPWStr )]
			string lParam );

		// NOTE: This definition is only used to access ParseDisplayName,
		// so the other definitions have been simplified to eliminate
		// unnecessary dependencies. They'll work, but it would be
		// better to use more specific types in many cases where int
		// or IntPtr arguments are used.
		[ComImport, Guid("000214E6-0000-0000-c000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IShellFolder
		{
			[PreserveSig]
			int ParseDisplayName(IntPtr hWnd, IntPtr bindingContext, 
				string displayName, out int chEaten, ref IntPtr idList, ref int attributes);
		
			[PreserveSig]
			int EnumObjects(IntPtr hWnd, int flags,  ref IntPtr enumList);

			[PreserveSig]
			int BindToObject(IntPtr idList, IntPtr bindingContext, ref Guid refiid, ref IShellFolder folder);
        
			[PreserveSig]
			int BindToStorage(ref IntPtr idList, IntPtr bindingContext, ref Guid riid, IntPtr pVoid);

			[PreserveSig]
			int CompareIDs(int lparam, IntPtr idList1, IntPtr idList2);
        
			[PreserveSig]
			int CreateViewObject(IntPtr hWnd, Guid riid, IntPtr pVoid);
        
			[PreserveSig]
			int GetAttributesOf(int count, ref IntPtr idList, out int attributes);

			[PreserveSig]
			int GetUIObjectOf(IntPtr hWnd, int count, ref IntPtr idList, 
				ref Guid riid, out int arrayInOut, ref IntPtr iUnknown);

			[PreserveSig]
			int GetDisplayNameOf(IntPtr idList, int flags, ref StringBuilder strRet);

			[PreserveSig]
			int SetNameOf(IntPtr hWnd, ref IntPtr idList,
				IntPtr pOLEString, int flags, ref IntPtr pItemIDList);
        
		}

		private delegate int BrowseCallbackHandler( 
			IntPtr hwnd,
			uint msg,
			uint lParam,
			uint dwData );

		#endregion
	}
}
