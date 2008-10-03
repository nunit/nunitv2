using System;
using System.IO;
using System.Collections.Generic;

namespace GenSyntax
{
    class Stanza
    {
        private string typeName;
        private List<string> comments = new List<string>();
        private List<GenSpec> genSpecs = new List<GenSpec>();

        public static Stanza Read(TextReader rdr)
        {
            Stanza stanza = new Stanza();

            string line = rdr.ReadLine();
            while (line != null && line != "%")
            {
                if (!line.StartsWith("#"))
                {
                    if (line.StartsWith("Type:"))
                        stanza.typeName = line.Substring(5).Trim();
                    else if (line.StartsWith("///"))
                        stanza.comments.Add(line);
                    else if (line.StartsWith("Gen:") || line.StartsWith("Obs:"))
                        stanza.genSpecs.Add(new GenSpec(line));
                    else
                        throw new ArgumentException("Invalid line in spec file" + Environment.NewLine + line);
                }

                line = rdr.ReadLine();
            }

            return stanza;
        }

        public void Generate(OutputWriter writer, string className)
        {
            foreach (GenSpec spec in genSpecs)
            {
                if (spec.ClassName == className)
                {
                    bool generic = spec.MethodName.IndexOf('<') > 0;
                    if (generic) writer.WriteLineNoTabs("#if NET_2_0");

                    foreach (string comment in comments)
                        writer.WriteLine(comment);

                    if (spec.Obsolete)
                        writer.WriteLine(string.Format(@"[Obsolete(""Use {0}"")]", spec.RetVal));

                    if ( className == "Is" || className == "Has" || className == "Text" || className == "Throws")
                        writer.WriteLine("public static {0} {1}", typeName, spec.MethodName);
                    else
                        writer.WriteLine("public {0} {1}", typeName, spec.MethodName);
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine(spec.Body);
                    writer.Indent--;
                    writer.WriteLine("}");
                    
                    if (generic) writer.WriteLineNoTabs("#endif");
                    
                    writer.WriteLine();
                }
            }
        }
    }
}
