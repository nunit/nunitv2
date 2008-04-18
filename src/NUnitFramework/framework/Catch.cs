using System;
using System.Text;

namespace NUnit.Framework
{
    public class Catch
    {
        public static Exception Exception(TestDelegate code)
        {
            try
            {
                code();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
