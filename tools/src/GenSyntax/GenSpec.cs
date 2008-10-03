using System;
using System.Collections.Generic;
using System.Text;

namespace GenSyntax
{
    class GenSpec
    {
        private string spec;
        private string specType;
        private string fullName;
        private string className;
        private string methodName;
        private string retval;
        private string body;

        public GenSpec(string spec)
        {
            this.spec = spec;

            int colon = spec.IndexOf(':');
            int arrow = spec.IndexOf( "=>", colon+1 );
            if (colon <= 0 || arrow <= 0)
                throw new ArgumentException(string.Format("Invalid generation spec: {0}", spec), "spec");

            this.specType = spec.Substring(0, colon + 1);
            this.fullName = spec.Substring(colon+1, arrow-colon-1);
            this.retval = spec.Substring(arrow + 2);

            int dot = fullName.IndexOf('.');

            this.className = fullName.Substring(0, dot).Trim();
            this.methodName = fullName.Substring(dot + 1).Trim();
            this.body = methodName.EndsWith(")")
                ? "return " + retval + ";"
                : "get { return " + retval + "; }";
        }

        public string MethodName
        {
            get { return methodName; }
        }

        public string ClassName
        {
            get { return className; }
        }

        public string Body
        {
            get { return body; }
        }

        public bool Obsolete
        {
            get { return specType == "Obs:"; }
        }

        public string RetVal
        {
            get { return retval; }
        }
    }
}
