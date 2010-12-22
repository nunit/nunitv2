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
using NUnit.Framework;

namespace NUnit.ProjectEditor.Tests
{
    [TestFixture]
    public class ExistingProjectTests
    {
        static readonly string xmlfile = "test.nunit";

        private ProjectModel project;
        private bool gotChangeNotice;

        [SetUp]
        public void SetUp()
        {
            project = new ProjectModel();
            project.CreateNewProject();
            project.Configs.Add("Debug");
            project.Configs.Add("Release");
            project.ProjectChanged += new CommandDelegate(OnProjectChange);

            gotChangeNotice = false;
        }

        [TearDown]
        public void EraseFile()
        {
            if (File.Exists(xmlfile))
                File.Delete(xmlfile);
        }

        private void OnProjectChange()
        {
            gotChangeNotice = true;
        }

        [Test]
        public void RenameConfigMakesProjectDirty()
        {
            project.Configs[0].Name = "New";
            Assert.IsTrue(project.HasUnsavedChanges);
        }

        [Test]
        public void RenameConfigFiresChangedEvent()
        {
            project.Configs[0].Name = "New";
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void RenamingActiveConfigChangesActiveConfigName()
        {
            project.Configs[0].Name = "New";
            Assert.AreEqual("New", project.ActiveConfigName);
        }

        [Test]
        public void RemoveConfigMakesProjectDirty()
        {
            project.Configs.Remove("Debug");
            Assert.IsTrue(project.HasUnsavedChanges);
        }

        [Test]
        public void RemoveConfigFiresChangedEvent()
        {
            project.Configs.Remove("Debug");
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void RemovingActiveConfigChangesActiveConfigName()
        {
            project.ActiveConfigName = "Debug";
            project.Configs.Remove("Debug");
            Assert.AreEqual("Release", project.ActiveConfigName);
        }

        [Test]
        public void SettingActiveConfigMakesProjectDirty()
        {
            project.ActiveConfigName = "Release";
            Assert.IsTrue(project.HasUnsavedChanges);
        }

        [Test]
        public void SettingActiveConfigFiresChangedEvent()
        {
            project.ActiveConfigName = "Release";
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void CanSetActiveConfig()
        {
            project.ActiveConfigName = "Release";
            Assert.AreEqual("Release", project.ActiveConfigName);
        }

        [Test]
        public void CanAddAssemblies()
        {
            project.Configs["Debug"].Assemblies.Add(Path.GetFullPath(@"bin\debug\assembly1.dll"));
            project.Configs["Debug"].Assemblies.Add(Path.GetFullPath(@"bin\debug\assembly2.dll"));
            project.Configs["Release"].Assemblies.Add(Path.GetFullPath(@"bin\debug\assembly3.dll"));

            Assert.AreEqual(2, project.Configs.Count);
            Assert.AreEqual(2, project.Configs["Debug"].Assemblies.Count);
            Assert.AreEqual(1, project.Configs["Release"].Assemblies.Count);
        }

        //[Test]
        //public void AddingAssemblyFiresChangedEvent()
        //{
        //    project.Configs["Debug"].Assemblies.Add("assembly1.dll");
        //    Assert.IsTrue(gotChangeNotice);
        //}

        //[Test]
        //public void RemoveAssemblyFiresChangedEvent()
        //{
        //    project.Configs["Debug"].Assemblies.Add("assembly1.dll");
        //    gotChangeNotice = false;
        //    project.Configs["Debug"].Assemblies.Remove("assembly1.dll");
        //    Assert.IsTrue(gotChangeNotice);
        //}

        [Test]
        public void CanSaveAndLoadProject()
        {
            project.Save(xmlfile);

            Assert.IsTrue(File.Exists(xmlfile));

            ProjectModel project2 = new ProjectModel(xmlfile);
            project2.Load();

            Assert.AreEqual(project.Name, project2.Name);
            Assert.AreEqual(2, project2.Configs.Count);
            Assert.IsTrue(project2.Configs.Contains("Debug"));
            Assert.IsTrue(project2.Configs.Contains("Release"));
        }

        [Test]
        public void CanSaveAndLoadProjectWithAssemblies()
        {
            project.Configs[0].Assemblies.Add(@"bin\debug\assembly1.dll");
            project.Configs[0].Assemblies.Add(@"bin\debug\assembly2.dll");

            project.Configs[1].Assemblies.Add(@"bin\release\assembly1.dll");
            project.Configs[1].Assemblies.Add(@"bin\release\assembly2.dll");

            project.Save(xmlfile);

            Assert.IsTrue(File.Exists(xmlfile));

            ProjectModel project2 = new ProjectModel(xmlfile);
            project2.Load();

            Assert.AreEqual(2, project2.Configs.Count);

            Assert.AreEqual(2, project2.Configs[0].Assemblies.Count);
            Assert.AreEqual(@"bin\debug\assembly1.dll", project2.Configs[0].Assemblies[0]);
            Assert.AreEqual(@"bin\debug\assembly2.dll", project2.Configs[0].Assemblies[1]);

            Assert.AreEqual(2, project2.Configs[1].Assemblies.Count);
            Assert.AreEqual(@"bin\release\assembly1.dll", project2.Configs[1].Assemblies[0]);
            Assert.AreEqual(@"bin\release\assembly2.dll", project2.Configs[1].Assemblies[1]);
        }
    }
}
