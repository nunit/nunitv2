// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.UiKit
{
	/// <summary>
	/// The TextDisplay interface is implemented by object - generally
	/// controls - that display text.
	/// </summary>
	public interface TextDisplay
	{
		/// <summary>
		/// Gets and sets the text to be displayed
		/// </summary>
		string Text { get; set; }

		/// <summary>
		/// Clears the display
		/// </summary>
		void Clear();

		/// <summary>
		/// Appends text to the display
		/// </summary>
		/// <param name="text">The text to append</param>
		void AppendText( string text );
	}
}
