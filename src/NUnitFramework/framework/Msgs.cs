// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

using System;

namespace NUnit.Framework
{
    /// <summary>
    /// This class the messages used by NUnitLite as well as the
    /// prefixes used on some of the message lines.
    /// </summary>
    public class Msgs
    {
        // Prefixes used in all failure messages. All must be the same
        // length, which is held in the PrefixLength field. Should not
        // contain any tabs or newline characters.

		/// <summary>
		/// Prefix used for the expected value line of a message
		/// </summary>
        public static readonly string Pfx_Expected = "  Expected: ";
		/// <summary>
		/// Prefix used for the actual value line of a message
		/// </summary>
        public static readonly string Pfx_Actual = "  But was:  ";
        public static readonly string Pfx_Tolerance = " within ";
		/// <summary>
		/// Prefix used when listing missing elements
		/// </summary>
        public static readonly string Pfx_Missing = "  Missing:  ";
		/// <summary>
		/// Prefix used when listing extra elements
		/// </summary>
        public static readonly string Pfx_Extra = "  Extra:    ";
		/// <summary>
		/// Length of a message prefix
		/// </summary>
        public static readonly int PrefixLength = Pfx_Expected.Length;
	}
}
