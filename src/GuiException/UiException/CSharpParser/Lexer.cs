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
    /// Splits a text formated as C# code into a list of identified tokens.
    /// </summary>
    public class Lexer
    {
        /// <summary>
        /// Reading position in the current text.
        /// </summary>
        private int _position;

        /// <summary>
        /// Text where to fetch tokens.
        /// </summary>
        private string _text;

        /// <summary>
        /// Last identified token.
        /// </summary>
        private InternalToken _token;

        /// <summary>
        /// Special character sequences.
        /// </summary>
        private SequenceCollection _sequences;

        /// <summary>
        /// Builds a new instance of Lexer.
        /// </summary>
        public Lexer()
        {
            _position = 0;
            _text = "";

            _sequences = new SequenceCollection();

            // All sequences must be added from the longest to the shortest.

            // Here: definition of two lengthed sequences
            AppendsSequence("\\\"", LexerTag.Text);
            AppendsSequence("\\\'", LexerTag.Text);
            AppendsSequence("/*", LexerTag.CommentC_Open);
            AppendsSequence("*/", LexerTag.CommentC_Close);
            AppendsSequence("//", LexerTag.CommentCpp);

            // Here: definition of one lengthed sequences
            AppendsSequence("\\", LexerTag.Text);
            AppendsSequence(" ", LexerTag.Separator);
            AppendsSequence("\t", LexerTag.Separator);
            AppendsSequence("\r", LexerTag.Separator);
            AppendsSequence(".", LexerTag.Separator);
            AppendsSequence(";", LexerTag.Separator);
            AppendsSequence("[", LexerTag.Separator);
            AppendsSequence("]", LexerTag.Separator);
            AppendsSequence("(", LexerTag.Separator);
            AppendsSequence(")", LexerTag.Separator);
            AppendsSequence("#", LexerTag.Separator);
            AppendsSequence(":", LexerTag.Separator);
            AppendsSequence("<", LexerTag.Separator);
            AppendsSequence(">", LexerTag.Separator);
            AppendsSequence(",", LexerTag.Separator);
            AppendsSequence("\n", LexerTag.EndOfLine);
            AppendsSequence("'", LexerTag.SingleQuote);
            AppendsSequence("\"", LexerTag.DoubleQuote);

            return;
        }

        /// <summary>
        /// Clear all previously defined sequences.
        /// </summary>
        protected void Clear()
        {
            _sequences.Clear();

            return;
        }

        /// <summary>
        /// Appends the given sequence to the list of all sequences to be
        /// classified a LexerTag.
        ///   Caller must give all sequences from the longest to the shortest.
        /// Failing to this requirement results in an exception.
        ///   All sequences must be unique.
        /// </summary>
        /// <param name="sequence">The sequence to be appended to the list.</param>
        /// <param name="tag">The classification for this sequence.</param>
        protected void AppendsSequence(string sequence, LexerTag tag)
        {
            TraceExceptionHelper.CheckNotNull(sequence, "sequence");

            TraceExceptionHelper.CheckTrue(sequence.Length > 0,
                "Sequence must not be empty.", "sequence");

            if (_sequences.Count > 0)
            {
                TraceExceptionHelper.CheckTrue(
                    _sequences[_sequences.Count - 1].Length >= sequence.Length,
                    "Sequences must be appended from the longest to the shortest.",
                    "sequence");
            }

            TraceExceptionHelper.CheckFalse(
                _sequences.Contains(sequence),
                String.Format("Sequence '{0}' is already defined.", sequence), "sequence");

            _sequences.Add(sequence, tag);

            return;
        }

        /// <summary>
        /// Setup the text to be splitted in tokens. 
        /// 
        /// Client code must call Next() first before getting
        /// the first available token (if any).
        /// </summary>
        public void Parse(string codeCSharp)
        {
            TraceExceptionHelper.CheckNotNull(codeCSharp, "text");          

            _text = codeCSharp;
            _position = 0;

            return;
        }

        /// <summary>
        /// Gets the token identifed after a call to Next().
        /// The value may be null if the end of the text has been reached.
        /// </summary>
        public LexToken CurrentToken {
            get { return (_token); }
        }

        /// <summary>
        /// Checks whether there are none visited tokens.
        /// </summary>
        public bool HasNext() {
            return (_position < _text.Length);
        }

        /// <summary>
        /// Call this method to visit iteratively all tokens in the source text.
        /// Each time a token has been identifier, the method returns true and the
        /// identified Token is place under the CurrentToken property.
        ///   When there is not more token to visit, the method returns false
        /// and null is set in CurrentToken property.
        /// </summary>
        public bool Next()
        {
            char c;         

            _token = null;

            if (!HasNext())
                return (false);

            _token = new InternalToken(_position);                        
            
            while (_position < _text.Length)
            {                
                c = _text[_position];
                _token.AppendsChar(c);

                // The test below checks whether token contains one of
                // predefined sequences. After pushing characters several times,
                // token eventually looks like: "[data]SEQUENCE", where [data] refers
                // to normal text, and SEQUENCE to a predefined sequence.
                //   When this happens, algorithm makes two things:
                // firstly, it splits token's data part from the SEQUENCE part. This makes
                // token contains only [data].
                // secondly, it positions the reading cursor at the starting startingPosition of SEQUENCE.
                // So it can returns the token, and the SEQUENCE value is prepared to be returned
                // as well.
                if (_sequences.MatchesWith(_token.Text))
                {                    
                    if (_token.Text.Length > _sequences.MatchingSequence.Length)
                    {
                        // This case occurs when token looks like: [data]SEQUENCE
                        // Now: we want to remove from token the SEQUENCE part and
                        // to return only the [data] part.
                        // We position cursor at the start of SEQUENCE.

                        _token.PopChars(_sequences.MatchingSequence.Length);
                        _position += 1 - _sequences.MatchingSequence.Length;
                    }
                    else
                    {
                        // this case occurs when token contains only the SEQUENCE part.
                        // updates LexerTag from the one that has been matched with.

                        _position ++;
                        _token.SetLexerTag(_sequences.MatchingLexerTag);
                    }

                    return (true);
                }                                             

                _position++;
            }

            return (true);
        }

        #region InternalToken

        class InternalToken :
            LexToken
        {
            /// <summary>
            /// Builds a concrete instance of LexToken. By default, created instance
            /// are setup with LexerTag.Text value.
            /// </summary>
            /// <param name="startingPosition">The starting startingPosition of this token in the text.</param>
            public InternalToken(int index)
            {
                _tag = LexerTag.Text;
                _text = "";
                _start = index;

                return;
            }

            /// <summary>
            /// Appends this character to this token.
            /// </summary>
            public void AppendsChar(char c) {
                _text += c;
            }

            /// <summary>
            /// Removes the "count" ending character of this token.
            /// </summary>
            public void PopChars(int count)
            {
                _text = _text.Remove(_text.Length - count);
            }

            /// <summary>
            /// Set a new value to the Tag property.
            /// </summary>
            public void SetLexerTag(LexerTag tag)
            {
                _tag = tag;
            }
        }

        #endregion

        #region SequenceCollection

        /// <summary>
        /// Holds a list of strings that corresponds to pre-defined tokens.
        /// </summary>
        class SequenceCollection
        {
            private List<string> _sequences;
            private List<LexerTag> _attrs;
            private int _index;

            public SequenceCollection()
            {
                _sequences = new List<string>();
                _attrs = new List<LexerTag>();
                _index = -1;

                return;
            }

            public int Count
            {
                get { return (_sequences.Count); }
            }

            public string this[int index]
            {
                get { return (_sequences[index]); }
            }

            public void Clear()
            {
                _sequences.Clear();
                _attrs.Clear();
                _index = -1;
            }

            public bool Contains(string sequence)
            {
                return (_sequences.Contains(sequence));
            }

            public void Add(string sequence, LexerTag attr)
            {
                _sequences.Add(sequence);
                _attrs.Add(attr);

                return;
            }

            public bool MatchesWith(string token)
            {
                int i;

                _index = -1;

                // loop on each predefined sequence and test whether
                // there is a match with the end part of token and the
                // current sequence.
                //   The use of EndsWith() may be counter intuitive at
                // first glance. But the algorithm builds tokens in such way
                // that normal data are placed at the beginning and sequence
                // (if any) at the end.
                // => token = dataSEQUENCE
                // where:
                // -data: is normal text
                // -SEQUENCE: is a predefined text.
                // Therefore, we use EndsWith() to locate a sequence (if any) in
                // token.

                // note: the loop below relies on the assumption that sequences are
                // visited from the longest to the shortest.

                for (i = 0; i < _sequences.Count; ++i)
                    if (token.EndsWith(_sequences[i]))
                        break;

                if (i < _sequences.Count)
                    _index = i;

                return (_index == i);
            }

            public string MatchingSequence
            {
                get
                {
                    TraceExceptionHelper.CheckTrue(_index > -1, "no match found", "");
                    return (_sequences[_index]);
                }
            }

            public LexerTag MatchingLexerTag
            {
                get
                {
                    TraceExceptionHelper.CheckTrue(_index > -1, "no match found", "");
                    return (_attrs[_index]);
                }
            }
        }

        #endregion
    }
}
