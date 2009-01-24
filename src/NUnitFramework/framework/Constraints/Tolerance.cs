// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// Modes in which the tolerance value for a comparison can
    /// be interpreted.
    /// </summary>
    public enum ToleranceMode
    {
        /// <summary>
        /// The tolerance was created with a value, without specifying 
        /// how the value would be used. This is used to prevent setting
        /// the mode more than once and is generally changed to Linear
        /// upon execution of the test.
        /// </summary>
        None,
        /// <summary>
        /// The tolerance is used as a numeric range within which
        /// two compared values are considered to be equal.
        /// </summary>
        Linear,
        /// <summary>
        /// Interprets the tolerance as the percentage by which
        /// the two compared values my deviate from each other.
        /// </summary>
        Percent,
        /// <summary>
        /// Compares two values based in their distance in
        /// representable numbers.
        /// </summary>
        Ulps
    }

    public class Tolerance
    {
        private ToleranceMode mode;
        private object amount;

        private static readonly string ModeMustFollowTolerance =
            "Tolerance amount must be specified before setting mode";
        private static readonly string MultipleToleranceModes =
            "Tried to use multiple tolerance modes at the same time";
        private static readonly string NumericToleranceRequired =
            "A numeric tolerance is required";

        public static Tolerance Empty
        {
            get { return new Tolerance(0, ToleranceMode.None); }
        }

        public Tolerance(object amount) : this(amount, ToleranceMode.Linear) { }

        private Tolerance(object amount, ToleranceMode mode)
        {
            this.amount = amount;
            this.mode = mode;
        }

        public ToleranceMode Mode
        {
            get { return this.mode; }
        }

        private void CheckLinearAndNumeric()
        {
            if (mode != ToleranceMode.Linear)
                throw new InvalidOperationException(mode == ToleranceMode.None
                    ? ModeMustFollowTolerance
                    : MultipleToleranceModes);

            if (!Numerics.IsNumericType(amount))
                throw new InvalidOperationException(NumericToleranceRequired);
        }

        public object Value
        {
            get { return this.amount; }
        }

        public Tolerance Percent
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(this.amount, ToleranceMode.Percent);
            }
        }

        public Tolerance Ulps
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(this.amount, ToleranceMode.Ulps);
            }
        }

        public Tolerance Days
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromDays(Convert.ToDouble(amount)));
            }
        }

        public Tolerance Hours
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromHours(Convert.ToDouble(amount)));
            }
        }

        public Tolerance Minutes
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromMinutes(Convert.ToDouble(amount)));
            }
        }

        public Tolerance Seconds
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromSeconds(Convert.ToDouble(amount)));
            }
        }

        public Tolerance Milliseconds
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromMilliseconds(Convert.ToDouble(amount)));
            }
        }

        public Tolerance Ticks
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromTicks(Convert.ToInt64(amount)));
            }
        }

        public bool IsEmpty
        {
            get { return mode == ToleranceMode.None; }
        }
    }
}
