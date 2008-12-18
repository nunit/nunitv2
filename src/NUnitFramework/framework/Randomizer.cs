using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Framework
{
    public class Randomizer : Random
    {
        #region Static Members
        private static Random seedGenerator = new Random();

        private static Hashtable randomizers = new Hashtable();

        public static int RandomSeed
        {
            get { return seedGenerator.Next(); }
        }

        public static Randomizer GetRandomizer(MemberInfo member)
        {
            Randomizer r = (Randomizer)randomizers[member];

            if ( r == null )
                randomizers[member] = r = new Randomizer();

            return r;
        }

        public static Randomizer GetRandomizer(ParameterInfo parameter)
        {
            return GetRandomizer(parameter.Member);
        }
        #endregion

        #region Constructors
        public Randomizer() : base(RandomSeed) { }

        public Randomizer(int seed) : base(seed) { }
        #endregion

        #region Public Methods
        public double[] GetDoubles(int count)
        {
            double[] rvals = new double[count];

            for (int index = 0; index < count; index++)
                rvals[index] = NextDouble();

            return rvals;
        }

        public double[] GetDoubles(double min, double max, int count)
        {
            double range = max - min;
            double[] rvals = new double[count];

            for (int index = 0; index < count; index++)
                rvals[index] = NextDouble() * range + min;

            return rvals;
        }

        public int[] GetInts(int min, int max, int count)
        {
            int[] ivals = new int[count];

            for (int index = 0; index < count; index++)
                ivals[index] = Next(min, max);

            return ivals;
        }
        #endregion
    }
}
