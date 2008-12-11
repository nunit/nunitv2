// ----------------------------------------------------------------
// ExceptionBrowser
// Version 1.0.0
// Copyright 2008, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.UiException.CSharpParser
{
    /// <summary>
    /// This enum indicate the kind of a string sequence.
    /// </summary>
    public enum ClassificationTag : byte
    {
        /// <summary>
        /// The string refer to C# source code.
        /// </summary>
        Code = 0,           // 0

        /// <summary>
        /// The string refers to C# keywords.
        /// </summary>
        Keyword = 1,        // 1

        /// <summary>
        /// The string refers to C# comments.
        /// </summary>
        Comment = 2,        // 2

        /// <summary>
        /// The string refers to a string/char value.
        /// </summary>
        String = 3          // 3
    }

    /// <summary>
    /// Keep tracks of the link between a string and a smState tag
    /// value to provide basic support for syntax coloring.
    /// </summary>
    public class CSToken
    {
        /// <summary>
        /// The string held by this token.
        /// </summary>
        protected string _text; 

        /// <summary>
        /// The matching tag.
        /// </summary>
        protected ClassificationTag _tag;

        /// <summary>
        /// Starting startingPosition of the string.
        /// </summary>
        protected int _indexStart;

        /// <summary>
        /// This class cannot be build directly.
        /// </summary>
        protected CSToken()
        {
            // this class requires subclassing
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        public string Text
        {
            get { return (_text); }
        }

        /// <summary>
        /// Get the string's smState value.
        /// </summary>
        public ClassificationTag Tag
        {
            get { return (_tag); }
        }

        /// <summary>
        /// Gets the string's starting startingPosition.
        /// </summary>
        public int IndexStart
        {
            get { return (_indexStart); }
        }

        /// <summary>
        /// Returns true if 'obj' is an instance of CSToken 
        /// that contains same data that the current instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            CSToken token;

            if (obj == null || !(obj is CSToken))
                return (false);

            token = obj as CSToken;

            return (Text == token.Text &&
                    Tag == token.Tag);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return (String.Format(
                "CSToken {Text='{0}', Tag={1}}",
                Text,
                Tag));
        }
    }
}
