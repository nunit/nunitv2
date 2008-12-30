// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Reflection;
using System.Text;

namespace NUnit.Core
{
    public class MethodHelper
    {
        public static string GetDisplayName(MethodInfo method, object[] arglist)
        {
            StringBuilder sb = new StringBuilder(method.Name);

#if NET_2_0
            if (method.IsGenericMethod)
            {
                sb.Append("<");
                int cnt = 0;
                foreach (Type t in method.GetGenericArguments())
                {
                    if (cnt++ > 0) sb.Append(",");
                    sb.Append(t.Name);
                }
                sb.Append(">");
            }
#endif

            if (arglist != null)
            {
                sb.Append("(");

                for (int i = 0; i < arglist.Length; i++)
                {
                    if (i > 0) sb.Append(",");
                    sb.Append(GetDisplayString(arglist[i]));
                }

                sb.Append(")");
            }

            return sb.ToString();
        }

        private static string GetDisplayString(object arg)
        {
            string display = arg == null ? "null" : arg.ToString();

            if (arg is double)
            {
                double d = (double)arg;

                if (double.IsNaN(d))
                    display = "double.NaN";
                else if (double.IsPositiveInfinity(d))
                    display = "double.PositiveInfinity";
                else if (double.IsNegativeInfinity(d))
                    display = "double.NegativeInfinity";
                else if (d == double.MaxValue)
                    display = "double.MaxValue";
                else if (d == double.MinValue)
                    display = "double.MinValue";
                else
                {
                    if (display.IndexOf('.') == -1)
                        display += ".0";
                    display += "d";
                }
            }
            else if (arg is float)
            {
                float f = (float)arg;

                if (float.IsNaN(f))
                    display = "float.NaN";
                else if (float.IsPositiveInfinity(f))
                    display = "float.PositiveInfinity";
                else if (float.IsNegativeInfinity(f))
                    display = "float.NegativeInfinity";
                else if (f == float.MaxValue)
                    display = "float.MaxValue";
                else if (f == float.MinValue)
                    display = "float.MinValue";
                else
                {
                    if (display.IndexOf('.') == -1)
                        display += ".0";
                    display += "f";
                }
            }
            else if (arg is decimal)
            {
                decimal d = (decimal)arg;
                if (d == decimal.MinValue)
                    display = "decimal.MinValue";
                else if (d == decimal.MaxValue)
                    display = "decimal.MaxValue";
                else
                    display += "m";
            }
            else if (arg is long)
            {
                long l = (long)arg;
                if (l == long.MinValue)
                    display = "long.MinValue";
                else if (l == long.MinValue)
                    display = "long.MaxValue";
                else
                    display += "L";
            }
            else if (arg is ulong)
            {
                ulong ul = (ulong)arg;
                if (ul == ulong.MinValue)
                    display = "ulong.MinValue";
                else if (ul == ulong.MinValue)
                    display = "ulong.MaxValue";
                else
                    display += "UL";
            }
            else if (arg is string)
            {
                display = "\"" + display + "\"";
            }
            else if (arg is int)
            {
                int ival = (int)arg;
                if (ival == int.MaxValue)
                    display = "int.MaxValue";
                else if (ival == int.MinValue)
                    display = "int.MinValue";
            }

            return display;
        }
    }
}
