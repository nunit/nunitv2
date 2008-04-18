using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Syntax.CSharp
{
	public class Throws
	{
		public static ThrowsConstraint Exception( Type type )
		{
			return new ThrowsConstraint( type );
		}

		public static ThrowsConstraint Exception(Type type, Constraint constraint)
		{
			return new ThrowsConstraint(type, constraint);
		}

#if NET_2_0
        public static ThrowsConstraint Exception<T>()
        {
            return new ThrowsConstraint(typeof(T));
        }

        public static ThrowsConstraint Exception<T>( Constraint constraint )
        {
            return new ThrowsConstraint(typeof(T), constraint );
        }
#endif

		public static ThrowsNothingConstraint Nothing
		{
			get { return new ThrowsNothingConstraint(); }
		}
    }
}
