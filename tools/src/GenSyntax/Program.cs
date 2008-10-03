using System;
using System.IO;
using System.Collections.Generic;

namespace GenSyntax
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	class Program
	{
        static string InputFile;
        static List<string> GenOptions = new List<string>();

        static List<string> GenDefaults = new List<string>( 
            new string[] { "Is", "Has", "Text", "Throws", "ConstraintFactory", "ConstraintExpression" } );

        static StreamReader InputReader;
        static List<StreamWriter> OutputWriters = new List<StreamWriter>();

        static List<Stanza> SyntaxInfo = new List<Stanza>();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            try
            {
                if (ProcessArgs(args))
                {
                    ReadSyntaxInfo();

                    foreach (string option in GenOptions)
                        Generate(option);
                }
                else
                    Usage();
            }
            catch (CommandLineError ex)
            {
                Error(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Error(ex.Message);
            }
		}

        static bool ProcessArgs(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg == "-help")
                    return false;
                else if (arg.StartsWith("-gen:"))
                    GenOptions.Add(arg.Substring(5));
                else if (InputFile == null)
                    InputFile = arg;
                else
                    throw new CommandLineError(string.Format("Unknown option: {0}", arg));
            }

            if (InputFile == null) throw new CommandLineError("No input file provided");

            InputReader = new StreamReader(InputFile);

            if (GenOptions.Count == 0)
                GenOptions.AddRange(GenDefaults);

            return true;
        }

        static void ReadSyntaxInfo()
        {
            while (!InputReader.EndOfStream)
            {
                Stanza stanza = Stanza.Read(InputReader);
                SyntaxInfo.Add(stanza);
            }
        }

        static void Generate(string option)
        {
            string className;
            string fileName;

            int eq = option.IndexOf('=');
            if (eq > 0)
            {
                className = option.Substring(0, eq);
                fileName = option.Substring(eq + 1);
            }
            else
            {
                className = option;
                fileName = className + ".cs";
                if (className == "ConstraintFactory" || className == "ConstraintExpression")
                    fileName = Path.Combine("Constraints", fileName);
            }

            Console.WriteLine("Generating " + fileName);

            OutputWriter writer = new OutputWriter(className, fileName);

            writer.WriteFileHeader();
            writer.Indent += 2;

            foreach (Stanza stanza in SyntaxInfo)
                stanza.Generate(writer, className);

            writer.Indent -= 2;
            writer.WriteFileTrailer();

            writer.Close();
        }

        static void Help()
        {
            Console.Error.WriteLine("Generates C# code for NUnit syntax elements");
            Console.Error.WriteLine();
            Usage();
        }

        static void Error(string message)
        {
            Console.Error.WriteLine(message);
            Console.Error.WriteLine();
            Usage();
        }

        static void Usage()
        {
            Console.Error.WriteLine("Usage: GenSyntax <input_file> [ [ [-gen:<class_name>[=<file_name>] ] ...]");
            Console.Error.WriteLine();
            Console.Error.WriteLine("The <input_file> is required. If any -gen options are given, only the code");
            Console.Error.WriteLine("for the specified classes are generated. If <file_name> is not specified,");
            Console.Error.WriteLine("it defaults to the <class_name> with a .cs extension. If no -gen options");
            Console.Error.WriteLine("are used, all the classes named in the input file are generated.");
        }
	}

    class CommandLineError : Exception
    {
        public CommandLineError(string message) : base(message) { }
    }
}
