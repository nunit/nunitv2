// ----------------------------------------------------------------
// ErrorBrowser
// Copyright 2008-2009, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using NUnit.UiException.CodeFormatters;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// Helper class that keeps tracks of the part of text that is
    /// currently visible to the user. This class is used by CodeBox
    /// to make its graphical display.
    ///   The viewport is a rectangle that has a width and a height and
    /// located by its left upper corner. This class contains utility methods
    /// that help to get what part of text is visible according its current
    /// size and position.
    /// </summary>
    public class CodeViewport :
        IEnumerable
    {
        /// <summary>
        /// Fired any time the location of the left upper corner changes.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Fired when the Text property is set.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Fired when the startingPosition of the highlighted line is set.
        /// </summary>
        public event EventHandler HighLightChanged;

        /// <summary>
        /// A constant for computation with double values.
        /// </summary>
        private const double EPSILON = 10E-5;

        /// <summary>
        /// Stores the current viewport's size in pixels.
        /// </summary>
        private Size _size;

        /// <summary>
        /// Keeps tracks of the left upper corner of the viewport.
        /// </summary>
        private PointF _location;

        /// <summary>
        /// Stores the size in pixel of a character in the current
        /// monospace font.
        /// </summary>
        private SizeF _policeSize;

        /// <summary>
        /// Keeps tracks of the complete text to be displayed.
        /// </summary>
        private ITextManager _lines;        

        /// <summary>
        /// Store the current startingPosition of the highlighted line.
        /// </summary>
        private int _highLighedLineIndex;

        /// <summary>
        /// Builds a new instance of the viewport.
        /// </summary>
        public CodeViewport()
        {
            _lines = new DefaultTextManager();
            _location = new PointF(0, 0);
            _highLighedLineIndex = -1;

            return;
        }

        /// <summary>
        /// Gets or sets the location of the viewport's left upper corner.
        /// Setting a location fires 'LocationChanged' event.
        /// </summary>
        public PointF Location
        {
            get { return (_location); }
            set 
            {
                _location = value;

                if (LocationChanged != null)
                    LocationChanged(this, new EventArgs());

                return;
            }
        }

        /// <summary>
        /// Change the Viewport's location to the specified coordinates.
        /// However the coordinates are corrected so the Viewport never
        /// scrolls out of the text area.
        /// </summary>
        public void SetPosition(double x, double y)
        {            
            x = Math.Min(x, CharWidth * TextSource.MaxLength - Width);
            x = Math.Max(0, x);

            y = Math.Min(y, CharHeight * TextSource.LineCount - Height);
            y = Math.Max(0, y);            

            Location = new PointF((float)x, (float)y);

            return;
        }

        /// <summary>
        /// Gets the viewport's width in pixels.
        /// </summary>
        public int Width
        {
            get { return (_size.Width); }            
        }

        /// <summary>
        /// Gets the viewport's height in pixels.
        /// </summary>
        public int Height
        {
            get { return (_size.Height); }
        }

        /// <summary>
        /// Gets the number of visible lines according the current
        /// viewport's sizes and location.
        /// </summary>
        public int VisibleLines
        {
            get
            {
                int lineStart;
                int lineEnd;
                int visible;

                lineStart = GetLineIndexFromTextUpperBoundCoordinate(Location.Y);
                if (lineStart >= _lines.LineCount)
                    return (0);

                lineEnd = GetLineIndexFromTextUpperBoundCoordinate(Location.Y + _size.Height - EPSILON);

                if (lineEnd < 0)
                    return (0);

                visible = Math.Min(_lines.LineCount - 1, lineEnd) - lineStart + 1;

                if (Location.X + _size.Width < 0 ||
                    Location.X > _lines.MaxLength * _policeSize.Width)
                    return (0);

                return (visible); 
            }
        }

        /// <summary>
        /// Gives access to the underlying object that keeps tracks
        /// of the text to be displayed.
        /// </summary>
        public ITextManager TextSource
        {
            get { return (_lines); }
            set 
            {
                UiExceptionHelper.CheckNotNull(value, "value");

                _lines = value;

                if (TextChanged != null)
                    TextChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets the text managed by the underlying TextManager object.
        /// </summary>
        public string Text
        {
            get { return (_lines.Text); }          
        }

        /// <summary>
        /// This indexer returns a new instance of a PaintLineLocation object filled
        /// with all the required data to render the text at the specified startingPosition, according
        /// the current viewport's status.
        /// </summary>
        /// <param name="startingPosition">A zero based value in the range: [0 - VisibleLines[.</param>
        /// <returns>A PaintLineLocation instance that contains all the data to render
        /// the part of text at the given startingPosition.</returns>
        public PaintLineLocation this[int index]
        {
            get
            {
                int lineIndex;
                string lineText;
                PointF lineCrd;
                PointF offset;

                lineIndex = GetLineIndexFromTextUpperBoundCoordinate(Location.Y + _policeSize.Height * index);

                lineText = "";
                if (lineIndex >= 0)
                    lineText = _lines.GetTextAt(lineIndex);

                lineCrd = GetTextUpperBoundCoordinateFromLineIndex(lineIndex);
                offset = new PointF(lineCrd.X - Location.X, lineCrd.Y - Location.Y);

                return (new PaintLineLocation(lineIndex, lineText, offset, lineIndex == _highLighedLineIndex));
            }
        }    

        /// <summary>
        /// Gets or sets the startingPosition of the line that should be highlighted.
        /// Setting a value fires 'HighLightChanged' event.
        /// </summary>
        public int HighLightedLineIndex
        {
            get { return (_highLighedLineIndex); }
            set 
            { 
                _highLighedLineIndex = value;

                if (HighLightChanged != null)
                    HighLightChanged(this, new EventArgs());

                return;
            }
        }

        /// <summary>
        /// Gets the font's width in pixels.
        /// </summary>
        public double CharWidth
        {
            get { return (_policeSize.Width); }            
        }

        /// <summary>
        /// Gets the font's height in pixels.
        /// </summary>
        public double CharHeight
        {
            get { return (_policeSize.Height); }
        }

        /// <summary>
        /// Gets the X distance in pixels between the
        /// start of the text and the Viewport's left bound.
        /// </summary>
        public double RemainingLeft {
            get { return (Location.X); }
        }

        /// <summary>
        /// Gets the X distance in pixels between the
        /// end of the text and the Viewport's right bound.
        /// </summary>
        public double RemainingRight
        {
            get { return (TextSource.MaxLength * CharWidth - (Width + Location.X)); }
        }

        /// <summary>
        /// Gets the Y distance in pixels between the
        /// top of the text and the Viewport's top bound.
        /// </summary>
        public double RemainingTop {
            get { return (Location.Y); }
        }

        /// <summary>
        /// Gets the Y distance in pixels between the
        /// bottom of the text and the Viewport's bottom bound.
        /// </summary>
        public double RemainingBottom 
        {
            get { return (TextSource.LineCount * CharHeight - (Height + Location.Y)); }
        }

        /// <summary>
        /// Scrolls viewport's location so the left upper viewport coordinate
        /// matches the text location at the given startingPosition.
        /// </summary>
        /// <param name="lineIndex">Index of the line where to scroll to.</param>
        public void ScrollToLine(int lineIndex)
        {
            lineIndex = Math.Max(0, lineIndex);

            SetPosition(0, (float)(lineIndex * _policeSize.Height));

            return;
        }

        /// <summary>
        /// Sets a new size in pixels for the viewport. All values must
        /// be > 0.
        /// </summary>
        /// <param name="width">The new width in pixels.</param>
        /// <param name="height">The new height in pixels.</param>
        public void SetViewport(int width, int height)
        {
            UiExceptionHelper.CheckTrue(width > 0, "width must be > 0", "width");
            UiExceptionHelper.CheckTrue(height > 0, "height must be > 0", "height");

            _size.Width = width;
            _size.Height = height;

            return;
        }

        /// <summary>
        /// Notifies the viewport about the current's size font.
        /// All values must be > 0.
        /// </summary>
        /// <param name="width">Width in pixels of the current font.</param>
        /// <param name="height">Height in pixels of the current font.</param>
        public void SetCharSize(float width, float height)
        {
            UiExceptionHelper.CheckTrue(width > 0, "width must be > 0", "width");
            UiExceptionHelper.CheckTrue(height > 0, "height must be > 0", "height");

            _policeSize.Width = width;
            _policeSize.Height = height;

            return;
        }

        /// <summary>
        /// Utility method that computes the upper bound coordinate
        /// in pixels of the line of text at the given startingPosition.
        /// </summary>
        /// <param name="lineIndex">The line startingPosition for which the request is being made.</param>
        /// <returns>The upper bound coordinate in pixels of this line.</returns>
        protected PointF GetTextUpperBoundCoordinateFromLineIndex(int lineIndex)
        {
            return (new PointF(0, (float)(lineIndex * _policeSize.Height)));
        }

        /// <summary>
        /// Utility method that computes the line startingPosition from an upper bound coordinate.
        /// </summary>
        /// <param name="y">A value in pixels corresponding to an upper bound coordinate.</param>
        /// <returns>The startingPosition of the closed line.</returns>
        protected int GetLineIndexFromTextUpperBoundCoordinate(double y)
        {
            int res;

            res = (int)(y / _policeSize.Height);

            return (res);
        }

        #region IEnumerable Membres

        /// <summary>
        /// Gets an IEnumerator that iterates through each lines of text.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            List<PaintLineLocation> list;
            int count;
            int i;

            list = new List<PaintLineLocation>();
            count = VisibleLines;

            for (i = 0; i < count; ++i)
                list.Add(this[i]);

            return (list.GetEnumerator());
        }

        #endregion
    }
}
