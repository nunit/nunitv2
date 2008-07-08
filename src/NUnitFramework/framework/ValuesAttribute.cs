using System;
using System.Collections;

namespace NUnit.Framework
{
    /// <summary>
    /// ValuesAttribute is used to provide literal arguments for
    /// an individual parameter of a test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple=true, Inherited=false)]
    public class ValuesAttribute : Attribute
    {
        /// <summary>
        /// The collection of data to be returned. Must
        /// be set by any derived attribute classes.
        /// </summary>
        protected ICollection data;

        /// <summary>
        /// Construct with one argument
        /// </summary>
        /// <param name="arg1"></param>
        public ValuesAttribute(object arg1)
        {
            data = new object[] { arg1 };
        }

        /// <summary>
        /// Construct with two arguments
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public ValuesAttribute(object arg1, object arg2)
        {
            data = new object[] { arg1, arg2 };
        }

        /// <summary>
        /// Construct with three arguments
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public ValuesAttribute(object arg1, object arg2, object arg3)
        {
            data = new object[] { arg1, arg2, arg3 };
        }

        /// <summary>
        /// Construct with an array of arguments
        /// </summary>
        /// <param name="args"></param>
        public ValuesAttribute(params object[] args)
        {
            data = args;
        }

        /// <summary>
        /// Get the collection of values to be used as arguments
        /// </summary>
        public ICollection Values
        {
			get { return data; }
        }
    }

    /// <summary>
    /// RangeAttribute is used to supply a range of values to an
    /// individual parameter of a parameterized test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class RangeAttribute : ValuesAttribute
    {
        /// <summary>
        /// Construct a range of ints using default step of 1
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public RangeAttribute(int from, int to) : this(from, to, 1) { }

        /// <summary>
        /// Construct a range of ints specifying the step size 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(int from, int to, int step)
        {
            int count = (to - from) / step + 1;
            int[] range = new int[count];
            int index = 0;
            for (int val = from; index < count; val += step)
                range[index++] = val;
            this.data = range;
        }

        /// <summary>
        /// Construct a range of longs
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(long from, long to, long step)
        {
            long count = (to - from) / step + 1;
            long[] range = new long[count];
            int index = 0;
            for (long val = from; index < count; val += step)
                range[index++] = val;
            this.data = range;
        }

        /// <summary>
        /// Construct a range of doubles
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(double from, double to, double step)
        {
            double tol = step / 1000;
            int count = (int)((to - from) / step + tol + 1);
            double[] range = new double[count];
            int index = 0;
            for (double val = from; index < count; val += step)
                range[index++] = val;
            this.data = range;
        }

        /// <summary>
        /// Construct a range of floats
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(float from, float to, float step)
        {
            float tol = step / 1000;
            int count = (int)((to - from) / step + tol + 1);
            float[] range = new float[count];
            int index = 0;
            for (float val = from; index < count; val += step)
                range[index++] = val;
            this.data = range;
        }
    }

    /// <summary>
    /// RandomAttribute is used to supply a set of random values
    /// to a single parameter of a parameterized test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
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
