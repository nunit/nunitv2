using System;
using System.Collections.Generic;
using System.Text;

namespace GenSyntax
{
    class GenSpec
    {
        private string fullSpec;
        bool isVoid;

        private string specType;
        private string leftPart;
        private string className;
        private string methodName;
        private string attributes;
        private string rightPart;
        private string body;

        public GenSpec(string spec) : this(spec, false) { }

        public GenSpec(string spec, bool isVoid)
        {
            this.fullSpec = spec;
            this.isVoid = isVoid;

            int colon = spec.IndexOf(':');
            int arrow = spec.IndexOf("=>", colon + 1);
            if (colon <= 0 || arrow <= 0)
                throw new ArgumentException(string.Format("Invalid generation spec: {0}", spec), "spec");

            this.specType = spec.Substring(0, colon + 1);
            this.leftPart = spec.Substring(colon+1, arrow-colon-1);
            this.rightPart = spec.Substring(arrow + 2);

            int dot = leftPart.IndexOf('.');

            this.className = leftPart.Substring(0, dot).Trim();
            this.methodName = leftPart.Substring(dot + 1).Trim();

            int rbrack = className.LastIndexOf("]");
            if (rbrack > 0)
            {
                this.attributes = className.Substring(0, rbrack + 1);
                this.className = className.Substring(rbrack + 1);
            }
        }

        public string SpecType
        {
            get { return this.specType; }
        }

        public string LeftPart
        {
            get { return this.leftPart; }
        }

        public string RightPart
        {
            get { return this.rightPart; }
        }

        public string MethodName
        {
            get { return methodName; }
        }

        public string ClassName
        {
            get { return className; }
        }

        public string Attributes
        {
            get { return attributes; }
        }

        public bool IsGeneric
        {
            get
            { 
                return this.MethodName.IndexOf('<') > 0
                    || this.MethodName.IndexOf('?') > 0;
            }
        }

        public override string ToString()
        {
            return fullSpec;
        }
    }
}
