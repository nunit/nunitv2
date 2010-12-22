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

using System;
using System.IO;
using System.Xml;
using NUnit.Framework;

namespace NUnit.ProjectEditor.Tests
{
	[TestFixture]
	public class NewProjectTests
	{
        static readonly string xmlfile = "test.nunit";

        private ProjectModel project;

		[SetUp]
		public void SetUp()
		{
            project = new ProjectModel();
            project.CreateNewProject();
		}

        [TearDown]
        public void EraseFile()
        {
            if (File.Exists(xmlfile))
                File.Delete(xmlfile);
        }

        [Test]
		public void HasNoConfigs()
		{
			Assert.AreEqual( 0, project.Configs.Count );
            Assert.IsNull( project.ActiveConfigName );
		}

        [Test]
        public void IsNotDirty()
        {
            Assert.IsFalse(project.HasUnsavedChanges);
        }

        [Test]
        public void ProjectPathIsSameAsName()
        {
            Assert.AreEqual(Path.GetFullPath(project.Name), project.ProjectPath);
        }

        [Test]
        public void NameIsUnique()
        {
            ProjectModel anotherProject = new ProjectModel(xmlfile);
            Assert.AreNotEqual(project.Name, anotherProject.Name);
        }

        [Test]
        public void SaveMakesProjectNotDirty()
        {
            project.Configs.Add("Debug");
            project.Save(xmlfile);
            Assert.IsFalse(project.HasUnsavedChanges);
        }

        [Test]
        public void SaveSetsProjectPathAndName()
        {
            project.Save(xmlfile);
            Assert.AreEqual(Path.GetFullPath(xmlfile), project.ProjectPath);
            Assert.AreEqual("test", project.Name);
        }

        //[Test]
        //public void SaveSetsDefaultApplicationBase()
        //{
        //    project.Save(xmlfile);
        //    Assert.AreEqual(Path.GetDirectoryName(project.ProjectPath), project.BasePath);
        //}

        [Test]
        public void SaveSetsDefaultConfigurationFile()
        {
            Assert.AreEqual(project.Name + ".config", project.DefaultConfigurationFile);
            project.Save(xmlfile);
            Assert.AreEqual("test.config", project.DefaultConfigurationFile);
        }

        [Test]
        public void DefaultProjectName()
        {
            project.Save(xmlfile);
            Assert.AreEqual("test", project.Name);
        }

        [Test]
        public void LoadMakesProjectNotDirty()
        {
            project.Configs.Add("Debug");
            project.Save(xmlfile);
            ProjectModel project2 = new ProjectModel(xmlfile);
            project2.Load();
            Assert.IsFalse(project2.HasUnsavedChanges);
        }

        [Test]
        public void CanSetAppBase()
        {
            project.BasePath = "..";
            Assert.AreEqual("..", project.BasePath);
        }

        [Test]
        public void CanSetAutoConfig()
        {
            Assert.IsFalse(project.AutoConfig);
            project.AutoConfig = true;
            Assert.IsTrue(project.AutoConfig);
        }

        [Test]
        public void CanAddConfigs()
        {
            project.Configs.Add("Debug");
            project.Configs.Add("Release");
            Assert.AreEqual(2, project.Configs.Count);
        }

        //[Test]
        //public void AddConfigMakesProjectDirty()
        //{
        //    project.AddConfig("Debug");
        //    Assert.IsTrue(project.HasChanges);
        //}

        //[Test]
        //public void AddConfigFiresChangedEvent()
        //{
        //    project.AddConfig("Debug");
        //    Assert.IsTrue(gotChangeNotice);
        //}

        [Test]
        public void DefaultActiveConfig()
        {
            project.Configs.Add("Debug");
            Assert.AreEqual("Debug", project.ActiveConfigName);
        }

        [Test]
        public void CanSaveAndLoadProject()
        {
            project.Save(xmlfile);
            Assert.IsTrue(File.Exists(xmlfile));

            ProjectModel project2 = new ProjectModel(xmlfile);
            project2.Load();

            Assert.AreEqual(project.Name, project2.Name);
            Assert.AreEqual(0, project2.Configs.Count);
        }
	}
}
