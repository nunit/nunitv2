// ***********************************************************************
// Copyright (c) 2010 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.IO;
using NUnit.Framework;
//using NUnit.TestUtilities;

namespace NUnit.ProjectEditor.Tests
{
	// TODO: Some of these tests are really tests of VSProject and should be moved there.

	[TestFixture]
	public class NUnitProjectLoadXmlTests
	{
		static readonly string xmlfile = "MyProject.nunit";

        private ProjectModel project;

		[SetUp]
		public void SetUp()
		{
            project = new ProjectModel(xmlfile);
		}

		[TearDown]
		public void TearDown()
		{
			if ( File.Exists( xmlfile ) )
				File.Delete( xmlfile );
		}

		[Test]
		public void LoadEmptyProject()
		{
            project.LoadXml(NUnitProjectXml.EmptyProject);
            Assert.AreEqual(Path.GetFullPath(xmlfile), project.ProjectPath);
			Assert.AreEqual( 0, project.Configs.Count );
		}

        [Test]
        public void LoadEmptyConfigs()
        {
            project.LoadXml(NUnitProjectXml.EmptyConfigs);
            Assert.AreEqual(2, project.Configs.Count);
            Assert.AreEqual("Debug", project.Configs[0].Name);
            Assert.AreEqual("Release", project.Configs[1].Name);
        }

        [Test]
        public void LoadNormalProject()
        {
            project.LoadXml(NUnitProjectXml.NormalProject);
            Assert.AreEqual(2, project.Configs.Count);

            ProjectConfig config1 = project.Configs[0];
            Assert.AreEqual(2, config1.Assemblies.Count);
            Assert.AreEqual(
                "assembly1.dll",
                config1.Assemblies[0]);
            Assert.AreEqual(
                "assembly2.dll",
                config1.Assemblies[1]);

            ProjectConfig config2 = project.Configs[1];
            Assert.AreEqual(2, config2.Assemblies.Count);
            Assert.AreEqual(
                "assembly1.dll",
                config2.Assemblies[0]);
            Assert.AreEqual(
                "assembly2.dll",
                config2.Assemblies[1]);
        }

        [Test]
        public void LoadProjectWithManualBinPath()
        {
            project.LoadXml(NUnitProjectXml.ManualBinPathProject);
            Assert.AreEqual(1, project.Configs.Count);
            ProjectConfig config1 = project.Configs["Debug"];
            Assert.AreEqual("bin_path_value", config1.PrivateBinPath);
        }

        [Test]
        public void LoadProjectWithComplexSettings()
        {
            project.LoadXml(NUnitProjectXml.ComplexSettingsProject);
            Assert.AreEqual("bin", project.BasePath);
            Assert.AreEqual(ProcessModel.Separate, project.ProcessModel);
            Assert.AreEqual(DomainUsage.Multiple, project.DomainUsage);

            Assert.AreEqual(2, project.Configs.Count);

            ProjectConfig config1 = project.Configs[0];
            Assert.AreEqual(
                "debug",
                config1.BasePath);
            Assert.AreEqual("v2.0", config1.RuntimeFramework.ToString());
            Assert.AreEqual("2.0", config1.RuntimeFramework.ClrVersion.ToString(2));
            Assert.AreEqual(2, config1.Assemblies.Count);
            Assert.AreEqual(
                "assembly1.dll",
                config1.Assemblies[0]);
            Assert.AreEqual(
                "assembly2.dll",
                config1.Assemblies[1]);

            ProjectConfig config2 = project.Configs[1];
            Assert.AreEqual(2, config2.Assemblies.Count);
            Assert.AreEqual(
                "release",
                config2.BasePath);
            Assert.AreEqual("v4.0", config2.RuntimeFramework.ToString());
            Assert.AreEqual("4.0", config2.RuntimeFramework.ClrVersion.ToString(2));
            Assert.AreEqual(
                "assembly1.dll",
                config2.Assemblies[0]);
            Assert.AreEqual(
                "assembly2.dll",
                config2.Assemblies[1]);
        }
	}
}
