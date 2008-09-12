// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework
{
    /// <summary>
    /// RandomAttribute is used to supply a set of random values
    /// to a single parameter of a parameterized test.
    /// </summary>
    public class RandomAttribute : ValuesAttribute
    {
        /// <summary>
        /// Construct a set of doubles from 0.0 to 1.0,
        /// specifying only the count.
        /// </summary>
        /// <param name="count"></param>
        public RandomAttribute(int count)
        {
            double[] rvals = new double[count];
            Random random = new Random(0);
            for (int index = 0; index < count; index++)
                rvals[index] = random.NextDouble();
            this.data = rvals;
        }

        /// <summary>
        /// Construct a set of doubles from min to max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        public RandomAttribute(double min, double max, int count)
        {
            double range = max - min;
            double[] rvals = new double[count];
            Random random = new Random(0);
            for (int index = 0; index < count; index++)
                rvals[index] = random.NextDouble() * range + min;
            this.data = rvals;
        }

        /// <summary>
        /// Construct a set of ints from min to max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        public RandomAttribute(int min, int max, int count)
        {
            int[] rvals = new int[count];
            Random random = new Random(0);
            for (int index = 0; index < count; index++)
                rvals[index] = random.Next(min, max);
            this.data = rvals;
        }
    }
}
