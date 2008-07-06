using System;
using System.Collections;

namespace NUnit.Framework
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple=true, Inherited=false)]
    public class ValuesAttribute : Attribute
    {
        protected ICollection data;

        public ValuesAttribute(object arg1)
        {
            data = new object[] { arg1 };
        }

        public ValuesAttribute(object arg1, object arg2)
        {
            data = new object[] { arg1, arg2 };
        }

        public ValuesAttribute(object arg1, object arg2, object arg3)
        {
            data = new object[] { arg1, arg2, arg3 };
        }

        public ValuesAttribute(params object[] args)
        {
            data = args;
        }

        public ICollection Values
        {
			get { return data; }
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class RangeAttribute : ValuesAttribute
    {
        public RangeAttribute(int from, int to) : this(from, to, 1) { }

        public RangeAttribute(int from, int to, int step)
        {
            int count = (to - from) / step + 1;
            int[] range = new int[count];
            int index = 0;
            for (int val = from; index < count; val += step)
                range[index++] = val;
            this.data = range;
        }

        public RangeAttribute(long from, long to, long step)
        {
            long count = (to - from) / step + 1;
            long[] range = new long[count];
            int index = 0;
            for (long val = from; index < count; val += step)
                range[index++] = val;
            this.data = range;
        }

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

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class RandomAttribute : ValuesAttribute
    {
        public RandomAttribute(int count)
        {
            double[] rvals = new double[count];
            Random random = new Random(0);
            for (int index = 0; index < count; index++)
                rvals[index] = random.NextDouble();
            this.data = rvals;
        }

        public RandomAttribute(double min, double max, int count)
        {
            double range = max - min;
            double[] rvals = new double[count];
            Random random = new Random(0);
            for (int index = 0; index < count; index++)
                rvals[index] = random.NextDouble() * range + min;
            this.data = rvals;
        }

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
