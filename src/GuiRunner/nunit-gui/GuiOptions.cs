// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Gui
{
	using System;
	using System.Text;
	using Codeblast;

	public class GuiOptions : CommandLineOptions
	{
		private bool isInvalid = false; 

		[Option(Short="?", Description = "Display help")]
		public bool help = false;

		[Option(Description = "Project configuration to load")]
		public string config;

		[Option(Description = "Suppress loading of last project")]
		public bool noload;

		[Option(Description = "Automatically run the loaded project")]
		public bool run;

		[Option(Description = "Fixture to test")]
		public string fixture;

		[Option(Description = "List of categories to include")]
		public string include;

		[Option(Description = "List of categories to exclude")]
		public string exclude;

		[Option(Description = ".NET Framework version to execute with (eg 'v1.0.3705')")]
		public string framework;

		[Option(Description = "Language to use for the NUnit GUI")]
		public string lang;

		[Option(Description = "Erase any leftover cache files and exit")]
		public bool cleanup;

		public GuiOptions(String[] args) : base(args) 
		{}

		protected override void InvalidOption(string name)
		{ isInvalid = true; }

		public string Assembly
		{
			get 
			{
				return (string)Parameters[0];
			}
		}

		public bool IsAssembly
		{
			get 
			{
				return ParameterCount == 1;
			}
		}

		public bool HasInclude 
		{
			get 
			{
				return include != null && include.Length != 0;
			}
		}

		public bool HasExclude 
		{
			get 
			{
				return exclude != null && exclude.Length != 0;
			}
		}

		public bool Validate()
		{
			if ( isInvalid ) return false;

			if ( HasInclude && HasExclude ) return false;

			return NoArgs || ParameterCount <= 1;
		}

		public override string GetHelpText()
		{
			const string initialText =
				"NUNIT-GUI [inputfile] [options]\r\rRuns a set of NUnit tests from the console. You may specify\ran assembly or a project file of type .nunit as input.\r\rOptions:\r";

			const string finalText = 
				"\rOptions that take values may use an equal sign, a colon\ror a space to separate the option from its value.";

			return initialText + base.GetHelpText() + finalText;
		}

	}
}