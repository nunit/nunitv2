namespace NUnit.Util
{
	using System;
	using System.Drawing;

	/// <summary>
	/// FormSettings holds settings for NUnitForm
	/// </summary>
	public class FormSettings : SettingsGroup
	{
		private static readonly string NAME = "Form";

		private static readonly string WIDTH = "width";
		private static readonly int DEFAULT_WIDTH = 632;

		private static readonly string HEIGHT = "height";
		private static readonly int DEFAULT_HEIGHT = 432;
		
		private static readonly string XLOCATION = "x-location";
		private static readonly int DEFAULT_XLOCATION = 10;
		
		private static readonly string YLOCATION = "y-location";
		private static readonly int DEFAULT_YLOCATION = 10;

		public FormSettings( ) : base( NAME, UserSettings.GetStorageImpl( NAME ) ) { }

		public FormSettings( SettingsStorage storage ) : base( NAME, storage ) { }

		public FormSettings( SettingsGroup parent ) : base( NAME, parent ) { }

		private Point location = Point.Empty;
		private Size size = Size.Empty;

		public Point Location
		{
			get 
			{
				if ( location == Point.Empty )
				{
					int x = LoadIntSetting( XLOCATION, DEFAULT_XLOCATION );
					int y = LoadIntSetting( YLOCATION, DEFAULT_YLOCATION );
					location = new Point(x, y);
				}
				
				return location; 
			}
			set 
			{ 
				location = value;
				SaveSetting( XLOCATION, location.X );
				SaveSetting( YLOCATION, location.Y );
			}
		}

		public Size Size
		{
			get 
			{ 
				if ( size == Size.Empty )
				{
					int width = LoadIntSetting( WIDTH, DEFAULT_WIDTH );
					int height = LoadIntSetting( HEIGHT, DEFAULT_HEIGHT );
					size = new Size(width, height);
				}

				return size;
			}
			set
			{ 
				size = value;
				SaveIntSetting( WIDTH, size.Width );
				SaveIntSetting( HEIGHT, size.Height );
			}
		}
	}
}
