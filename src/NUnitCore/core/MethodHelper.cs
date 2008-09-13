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

                    object arg = arglist[i];
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
                        else
                        {
                            if (display.IndexOf('.') == -1)
                                display += ".0";
                            display += "f";
                        }
                    }
                    else if (arg is decimal) display += "m";
                    else if (arg is long) display += "L";
                    else if (arg is ulong) display += "UL";
                    else if (arg is string) display = "\"" + display + "\"";

                    sb.Append(display);
                }

                sb.Append(")");
            }

            return sb.ToString();
        }
    }
}
