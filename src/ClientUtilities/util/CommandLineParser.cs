using System;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for CommandLineParser.
	/// </summary>
	public class CommandLineParser
	{
		public static readonly string ASSEMBLY_PARM = "/assembly";
		public static readonly string FIXTURE_PARM = "/fixture";
		public static readonly string XML_PARM = "/xml";
		public static readonly string TRANSFORM_PARM = "/transform";

		private IList allowedParameters = new ArrayList();
		private Hashtable splitArgs = new Hashtable(); 
		private bool noArgs;

		public CommandLineParser(IList parms, string[] args)
		{
			allowedParameters = parms;
			Parse(args);
		}

		public CommandLineParser(string[] args)
		{
			allowedParameters.Add(ASSEMBLY_PARM);
			allowedParameters.Add(FIXTURE_PARM);
			allowedParameters.Add(XML_PARM);
			allowedParameters.Add(TRANSFORM_PARM);

			Parse(args);
		}

		private void Parse(string[] args)
		{
			if(args.Length == 0)
				noArgs = true;
			else
			{
				foreach(string arg in args)
				{
					int commandIndex = arg.IndexOf(":");
					if(commandIndex == -1) throw new CommandLineException("invalid parameter: " + arg);

					string key = arg.Substring(0, commandIndex);
					string commandValue = arg.Substring(commandIndex+1);

					if(key.Length == 0 || commandValue.Length == 0)
						throw new CommandLineException("invalid parameters:" + arg);

					splitArgs.Add(key, commandValue);
				}
			
				ValidateCommandLine(args);
			}
		}

		public bool NoArgs
		{
			get { return noArgs; }
		}


		public void ValidateCommandLine(string[] args)
		{
			ICollection collection = splitArgs.Keys;
			foreach(string key in collection)
			{
				if(!allowedParameters.Contains(key)) 
					throw new CommandLineException(
						string.Format("Parameter {0} is not allowed",key));
			}

			if(!(IsAssembly || IsFixture))
				throw new CommandLineException("invalid args");
			return;
		}

		public bool IsAssembly
		{
			get { return splitArgs.Contains(ASSEMBLY_PARM) && !splitArgs.Contains(FIXTURE_PARM); }
		}

		public string AssemblyName
		{
			get { return (string)splitArgs[ASSEMBLY_PARM]; }
		}

		public bool IsFixture
		{
			get { return splitArgs.Contains(ASSEMBLY_PARM) && splitArgs.Contains(FIXTURE_PARM); }
		}

		public string TestName
		{
			get { return (string)splitArgs[FIXTURE_PARM]; }
		}

		public bool IsXml
		{
			get { return splitArgs.Contains(XML_PARM); }
		}

		public string XmlFileName
		{
			get { return (string)splitArgs[XML_PARM]; }
		}

		public bool IsTransform
		{
			get { return splitArgs.Contains(TRANSFORM_PARM); }
		}

		public string TransformFileName
		{
			get { return (string)splitArgs[TRANSFORM_PARM]; }
		}
	}
}
