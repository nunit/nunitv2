using System;
using System.Collections;
using System.Reflection;
using System.Text;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class TestCaseParameterProvider : IParameterProvider 
    {
        /// <summary>
        /// Determine whether any ParameterSets
        /// are available for a method.
        /// </summary>
        /// <param name="method">A MethodInfo representing the a parameterized test</param>
        /// <returns>True if any are available, otherwise false.</returns>
        public bool HasParametersFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.TestCaseAttribute, false);
        }

        /// <summary>
        /// Return a list providing ParameterSets
        /// for use in running a test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetParametersFor(MethodInfo method)
        {
#if !NET_2_0
			ArrayList list = new ArrayList();
#endif
            Attribute[] attrs = Reflect.GetAttributes(method, NUnitFramework.TestCaseAttribute, false);

            ParameterInfo[] parameters = method.GetParameters();
            int argsNeeded = parameters.Length;

            foreach (Attribute attr in attrs)
            {
                ParameterSet parms = ParameterSet.FromDataSource(attr);
                if (parms.TestName == null && parms.Arguments.Length > 0)
                    parms.TestName = method.Name + GetArgumentString(parms.Arguments);

				if (parms.Arguments.Length == argsNeeded)
				{
					try
					{
						for (int i = 0; i < argsNeeded; i++)
							MakeArgumentCompatible(ref parms.Arguments[i], parameters[i].ParameterType);
					}
					catch (Exception)
					{
						// Do nothing - the incompatible argument will be reported
						// by NUnitTestCaseBuilder
					}
				}
#if NET_2_0
                    yield return parms;
            }
#else
					list.Add( parms );
			}

			return list;
#endif
        }
        
        private static void MakeArgumentCompatible(ref object arg, Type targetType)
        {
            if (arg != null && !targetType.IsAssignableFrom(arg.GetType()))
            {
                if (arg is DBNull)
                    arg = null;
                else if (arg is IConvertible && targetType != typeof(string) )
                    arg = Convert.ChangeType(arg, targetType);
            }
        }

		private static string GetArgumentString(object[] arglist)
		{
			StringBuilder sb = new StringBuilder("(");

			if (arglist != null)
				for (int i = 0; i < arglist.Length; i++)
				{
					if (i > 0) sb.Append(",");

					object arg = arglist[i];
					if (arg == null)
						sb.Append("null");
					else
						sb.Append(arg.ToString());
				}

			sb.Append(")");

			return sb.ToString();
		}
	}
}
