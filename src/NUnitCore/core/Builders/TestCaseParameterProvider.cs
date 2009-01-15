// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;
using System.Text;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class TestCaseParameterProvider : ITestCaseProvider 
    {
        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.TestCaseAttribute, false);
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method)
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

                //if (method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(object[]))
                //    parms.Arguments = new object[]{parms.Arguments};

                if (argsNeeded == 1 && method.GetParameters()[0].ParameterType == typeof(object[]))
                {
                    if (parms.Arguments.Length > 1 ||
                        parms.Arguments.Length == 1 && parms.Arguments[0].GetType() != typeof(object[]))
                    {
                        parms.Arguments = new object[] { parms.Arguments };
                    }
                }
                    

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
						// by ParameterizedTestCaseBuilder
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
                else if (arg is IConvertible && targetType != typeof(string))
                    arg = Convert.ChangeType(arg, targetType, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
