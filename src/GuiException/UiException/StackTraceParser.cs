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

namespace NUnit.UiException
{
    /// <summary>
    /// Parse a StackTrace value and converts it as a
    /// collection of ExceptionItem objects.
    /// </summary>
    public class StackTraceParser
    {
        /// <summary>
        /// Default source file extension.
        /// </summary>
        private const string DEFAULT_CSHARP_EXTENSION = ".cs";

        /// <summary>
        /// Output list build from the StackTrace analyze .
        /// </summary>
        private ExceptionItemCollection _items;

        /// <summary>
        /// The directory separator char of the current operating system.
        /// </summary>
        private char _dirSeparator;

        /// <summary>
        /// The current source file extension.
        /// </summary>
        private string _extension;

        /// <summary>
        /// Build a new instance of StackTraceParser.
        /// </summary>
        public StackTraceParser()            
        {
            _items = new ExceptionItemCollection();
            _dirSeparator = System.IO.Path.DirectorySeparatorChar;
            
            _extension = DEFAULT_CSHARP_EXTENSION;

            return;
        }
        
        /// <summary>
        /// Gets or sets the directory separator char
        /// to be used when analyzing the StackTrace.
        /// </summary>
        public char DirectorySeparator
        {
            get { return (_dirSeparator); }
            set { _dirSeparator = value; }
        }

        /// <summary>
        /// Gets or sets the file extension to be used
        /// when analyzing the StackTrace.
        /// </summary>
        public string FileExtension
        {
            get { return (_extension); }
            set 
            {
                TraceExceptionHelper.CheckNotNull(value, "FileExtension");
                _extension = value; 
            }
        }

        /// <summary>
        /// Gives access to the collection of ExceptionItem
        /// build during the analyze of the StackTrace.
        /// </summary>
        public ExceptionItemCollection Items
        {
            get { return (_items); }
        }

        /// <summary>
        /// Parse the given StackTrace and build the corresponding
        /// ExceptionItemCollection. Once invoked on a valid value, caller
        /// can access the built collection by calling Items property.
        /// 
        ///     In order to build successfully the collection, the Parse() method makes
        /// the following assumptions:
        ///     - The StackTrace may be composed of one or several lines of text,
        ///     - Each exception description ends with a trailing '\r\n' sequence,
        ///     - Each exception may contain: function, file and line data in this order
        ///     - function descriptions contain: '(' and ')' characters
        ///     - file descriptions use: DirectorySeparator and FileExtension value
        ///     - line descriptions are always put at the end of each line.
        /// </summary>
        /// <param name="stackTrace">The stack trace value.</param>
        public void Parse(string stackTrace)
        {
            DefaultTextManager lines;
            InternalStringRange functionRange;
            InternalStringRange fileRange;
            InternalStringRange lineRange;
            int lineNumber;
            ExceptionItem item;

            _items.Clear();

            lines = new DefaultTextManager();
            lines.Text = stackTrace;

            foreach (string line in lines)
            {
                functionRange = _extractFunction(line);
                fileRange = _extractFilename(line);
                lineRange = _extractLineNumber(line, fileRange.End);

                Int32.TryParse(lineRange.Text, out lineNumber);
                item = new ExceptionItem(fileRange.Text, functionRange.Text + "()", lineNumber);

                _items.Add(item);
            }            

            return;
        }

        /// <summary>
        /// Reads and return the function description (if any) from the
        /// current line of text.
        /// </summary>
        /// <param name="line">A line of text coming from a StackTrace description.</param>
        /// <returns>An object that represent a string.</returns>
        private InternalStringRange _extractFunction(string line)
        {
            InternalStringRange functionRange;
            char c;

            functionRange = new InternalStringRange(line);
            if ((functionRange.End = line.IndexOf("(")) == -1)
                return (functionRange);

            functionRange.End--;
            functionRange.Start = functionRange.End;
            c = line[functionRange.Start - 1];

            while (!Char.IsWhiteSpace(c) && functionRange.Start > 0)
            {
                functionRange.Start--;
                c = line[functionRange.Start - 1];
            }

            return (functionRange);
        }

        /// <summary>
        /// Reads and return the file description (if any) from the
        /// current line of text.
        /// </summary>
        /// <param name="line">A line of text coming from a StackTrace description.</param>
        /// <returns>An object that represent a string.</returns>
        private InternalStringRange _extractFilename(string line)
        {
            InternalStringRange fileRange;

            fileRange = new InternalStringRange(line);
            fileRange.End = line.LastIndexOf(_extension, StringComparison.InvariantCultureIgnoreCase);
            fileRange.End += _extension.Length - 1;
            fileRange.Start = line.IndexOf(_dirSeparator);

            while (fileRange.Start > 0 &&
                   !Char.IsWhiteSpace(line[fileRange.Start - 1]))
                fileRange.Start--;

            return (fileRange);
        }

        /// <summary>
        /// Reads and return the line description (if any) from the
        /// current line of text and the given starting position.
        /// </summary>
        /// <param name="line">A line of text coming from a StackTrace description.</param>
        /// <param name="startingPosition">The position from where starting looking for line description.</param>
        /// <returns>An object that represent a string.</returns>
        private InternalStringRange _extractLineNumber(string line, int startingPosition)
        {
            InternalStringRange lineRange;

            lineRange = new InternalStringRange(line);
            lineRange.Start = startingPosition;
            lineRange.End = line.Length - 1;
            while (lineRange.Start < line.Length &&
                !Char.IsDigit(line[lineRange.Start]))
                lineRange.Start++;

            return (lineRange);
        }       

        #region InternalStringRange

        /// <summary>
        /// Manages an interval range position in a text that may
        /// possibly be invalid (i.e.: start > end).
        /// </summary>
        class InternalStringRange
        {
            private string _text;
            private int _start;
            private int _end;

            /// <summary>
            /// Builds a new InternalStringRange instance.
            /// </summary>
            /// <param name="text">A text from where getting the final string.</param>
            public InternalStringRange(string text)
            {
                _text = text;
            }

            /// <summary>
            /// Gets or sets the starting position in the current text.
            /// </summary>
            public int Start
            {
                get { return (_start); }
                set { _start = value; }
            }

            /// <summary>
            /// Gets or sets the ending position in the current text.
            /// </summary>
            public int End
            {
                get { return (_end); }
                set { _end = value; }
            }

            /// <summary>
            /// Extract the sub string that match the current text interval.
            /// If the interval is not valid, null is returned.
            /// </summary>
            public string Text
            {
                get
                {
                    try {
                        return (_text.Substring(Start, End - Start + 1));
                    }
                    catch (Exception) {
                        // indexes were invalid
                        // nothing to do
                    }

                    return (null);
                }
            }

            public override string ToString()
            {
                return (Text);
            }
        }

        #endregion
    }
}
