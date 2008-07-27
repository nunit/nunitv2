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

                    if (arg is double || arg is float)
                    {
                        if (display.IndexOf('.') == -1)
                            display += ".0";
                        display += arg is double ? "d" : "f";
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
