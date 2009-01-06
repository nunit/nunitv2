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
    /// Helper class to build and setup CSCode instances from formatted C# texts.
    /// </summary>
    public class CSParser
    {
        /// <summary>
        /// The underlying data object of a CSCode instance.
        /// </summary>
        private CSCode.CodeInfo _info;

        /// <summary>
        /// Builds a new instance of CSParser.
        /// </summary>
        public CSParser()
        {
            _info = CSCode.NewCodeInfo();

            return;
        }

        /// <summary>
        /// Gets a new instance of CSCode.
        /// To get useful CSCode instances, caller should ensure
        /// that Parse() was invoked first.
        /// </summary>
        public CSCode CSCode
        {
            get { return (new ConcreteCSCode(_info)); }
        }

        /// <summary>
        /// Prepare input text for the parsing stage.
        /// </summary>
        /// <param name="text">The text to be pre-processed.</param>
        /// <returns>A string ready to be parsed.</returns>
        protected string PreProcess(string text)
        {
            if (text == null)
                return (null);

            // this replace tabulation by space sequences. The reason is
            // that the technique used to measure strings at rendering time
            // fail to measure '\t' correctly and lines of text containing those
            // characters are badly aligned.
            //  The simplest thing to fix that is to remove tabs altogether.

            return (text.Replace("\t", "    "));
        }

        /// <summary>
        /// Analyzes the input text as C# code. This method doesn't return anything.
        /// Callers may retrieve the result of this process by querying the CSCode property.
        ///   Passing null results in raising an exception.
        /// </summary>
        /// <param name="csharp">The text to be analyzed.</param>
        public void Parse(string csharp)
        {
            TokenClassifier classifier;
            ConcreteToken csToken;
            ClassificationTag tag;
            Lexer lexer;
            StringBuilder text;
            int tokenIndex;

            TraceExceptionHelper.CheckNotNull(csharp, "csharp");

            csharp = PreProcess(csharp);

            lexer = new Lexer();
            lexer.Parse(csharp);

            classifier = new TokenClassifier();
            text = new StringBuilder();
            csToken = null;
            tokenIndex = 0;

            // loop through each token in the text
            while (lexer.Next())
            {
                // classify the current token 
                tag = classifier.Classify(lexer.CurrentToken);

                // if the tag cannot be merged with current csToken
                // we flush csToken into _info and build a new instance
                // from the current tag.
                if (csToken == null ||
                    !csToken.CanMerge(_info.LineArray.Count, tag))
                {
                    _flushToken(csToken, _info);
                    csToken = new ConcreteToken(
                        lexer.CurrentToken.Text, tag,
                        lexer.CurrentToken.IndexStart,
                        _info.LineArray.Count);
                }

                // append token's text into text
                text.Append(lexer.CurrentToken.Text);

                // handle newline character. Appends tokenIndex to LineArray
                // and set tokenIndex to the start of the newline.
                if (lexer.CurrentToken.Text == "\n")
                {
                    _info.LineArray.Add(tokenIndex);
                    tokenIndex = _info.IndexArray.Count + 1;
                }
            }

            // flush terminal token
            _flushToken(csToken, _info);

            if (csToken != null &&
                _info.LineArray.Count == 0)
                _info.LineArray.Add(tokenIndex);

            //_info.Text = text.ToString();
            _info.Text = csharp;

            return;
        }

        /// <summary>
        /// Appends data in token at the end of output.
        /// </summary>
        /// <param name="token">Token to be merged with output.</param>
        /// <param name="output">Target location.</param>
        private void _flushToken(CSToken token, CSCode.CodeInfo output)
        {
            if (token == null)
                return;

            output.IndexArray.Add(token.IndexStart);
            output.TagArray.Add((byte)token.Tag);

            return;
        }

        #region ConcreteCSCode

        /// <summary>
        /// Implements CSCode.
        /// </summary>
        class ConcreteCSCode :
            CSCode
        {
            public ConcreteCSCode(CSCode.CodeInfo info)
            {
                _codeInfo = info;
            }
        }

        #endregion

        #region ConcreteToken

        /// <summary>
        /// Implements CSToken.
        /// </summary>
        class ConcreteToken :
            CSToken
        {
            private int _lineIndex;

            /// <summary>
            /// Builds and setup a new instance of CSToken.
            /// </summary>
            /// <param name="text">The text in this token.</param>
            /// <param name="tag">The smState tag.</param>
            /// <param name="indexStart">Starting startingPosition of the string from the beginning of the text.</param>
            /// <param name="lineIndex">The line startingPosition.</param>
            public ConcreteToken(string text, ClassificationTag tag, int indexStart, int lineIndex)
            {
                _text = text;
                _tag = tag;
                _indexStart = indexStart;
                _lineIndex = lineIndex;

                return;
            }

            /// <summary>
            /// Tests whether or not the given lineIndex and tag are compatible with
            /// the ones in the current Token.
            /// </summary>
            /// <param name="lineIndex">A line startingPosition.</param>
            /// <param name="tag">A smState tag.</param>
            /// <returns>A boolean that says whether these data are compatible.</returns>
            public bool CanMerge(int lineIndex, ClassificationTag tag)
            {
                return (_tag == tag && _lineIndex == lineIndex);
            }
        }

        #endregion
    }
}
