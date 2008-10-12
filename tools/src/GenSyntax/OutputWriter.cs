using System;
using System.IO;
using System.Reflection;

namespace GenSyntax
{
    class OutputWriter : System.CodeDom.Compiler.IndentedTextWriter
    {
        private string className;
        private string fileName;

        private StreamReader template;

        public OutputWriter(string className, string fileName) 
            : base(new StreamWriter(fileName)) 
        {
            this.className = className;
            this.fileName = fileName;

            Assembly assembly = GetType().Assembly;
            Stream stream = assembly.GetManifestResourceStream("GenSyntax.Templates." + className + ".template.cs");
            if (stream == null)
                stream = assembly.GetManifestResourceStream("GenSyntax.Templates.Default.template.cs");

            this.template = new StreamReader(stream);
        }

        public void WriteFileHeader()
        {
            string[] argList = Environment.GetCommandLineArgs();
            argList[0] = Path.GetFileName(argList[0]);
            string commandLine = string.Join(" ", argList);

            string line = template.ReadLine();
            while (line != null && line.IndexOf("$$GENERATE$$") < 0)
            {
                line = line.Replace("__CLASSNAME__", this.className)
                           .Replace("__COMMANDLINE__", commandLine);
                WriteLine(line);
                line = template.ReadLine();
            }
        }

        public void WriteFileTrailer()
        {
            while (!template.EndOfStream)
                WriteLine(template.ReadLine());
        }
    }
}
