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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace NUnit.ProjectEditor.Tests
{
    [TestFixture]
    public class ProjectPresenterTests
    {
        MockRepository mocks;

        private IProjectModel model;
        private IPropertyView view;
        private IMessageBoxCreator mbox;
        private PropertyPresenter presenter;

        [SetUp]
        public void Initialize()
        {
            mocks = new MockRepository();

            model = new ProjectModel();
            model.CreateNewProject();
            model.Configs.Add("Debug");
            view = mocks.Stub<IPropertyView>();
            mbox = mocks.DynamicMock<MessageBoxCreator>("Editor");

            mocks.ReplayAll();
            presenter = new PropertyPresenter(model, view, mbox);
        }

        private void RaisePropertyChangedEvent(string name)
        {
            view.Raise(x => x.PropertyChanged += null, view, new PropertyChangedEventArgs(name));
        }

        [Test]
        public void ProcessModelOptionsAreInitializedCorrectly()
        {
            Assert.That(view.ProcessModelOptions, Is.EqualTo(
                new string[] { "Default", "Single", "Separate", "Multiple" }));
        }

        [Test]
        public void RuntimesAreInitializedCorrectly()
        {
            Assert.That(view.RuntimeOptions, Is.EqualTo(
                new string[] { "Any", "Net", "Mono" }));
        }

        [Test]
        public void RuntimeVersionsAreInitializedCorrectly()
        {
            Assert.That(view.RuntimeVersionOptions, Is.EqualTo(
                new string[] { "1.0.3705", "1.1.4322", "2.0.50727", "4.0.21006" }));
        }

        [Test]
        public void PresenterSubscribesToPropertyChanges()
        {
            view.AssertWasCalled(x => x.PropertyChanged += Arg<PropertyChangedEventHandler>.Is.Anything);
        }

        [Test]
        public void WhenProjectModelIsChangedDomainUsageOptionsChanged()
        {
            view.ProcessModel = "Single";
            RaisePropertyChangedEvent("ProcessModel");
            Assert.That(view.DomainUsageOptions, Is.EqualTo(
                new string[] { "Default", "Single", "Multiple" }));

            view.ProcessModel = "Multiple";
            RaisePropertyChangedEvent("ProcessModel");
            Assert.That(view.DomainUsageOptions, Is.EqualTo(
                new string[] { "Default", "Single" }));
        }

        [Test]
        public void ChangingProcessModelUpdatesProject()
        {
            view.ProcessModel = "Multiple";
            RaisePropertyChangedEvent("ProcessModel");
            Assert.That(model.ProcessModel, Is.EqualTo(ProcessModel.Multiple));
        }

        [Test]
        public void ChangingDomainUsageUpdatesProject()
        {
            view.DomainUsage = "Multiple";
            RaisePropertyChangedEvent("DomainUsage");
            Assert.That(model.DomainUsage, Is.EqualTo(DomainUsage.Multiple));
        }

        [Test]
        public void ChangingProjectBaseUpdatesProject()
        {
            view.ProjectBase = "test.nunit";
            RaisePropertyChangedEvent("ProjectBase");
            Assert.That(model.BasePath, Is.EqualTo("test.nunit"));
        }

        [Test]
        public void ChangingRuntimeUpdatesProject()
        {
            model.Configs[0].RuntimeFramework = new RuntimeFramework(RuntimeType.Net, new Version("2.0.50727"));

            view.SelectedConfig = 0;
            view.Runtime = "Mono";
            view.RuntimeVersion = "1.1.4322";
            RaisePropertyChangedEvent("Runtime");
            RuntimeFramework framework = model.Configs[0].RuntimeFramework;
            Assert.That(framework.Runtime, Is.EqualTo(RuntimeType.Mono));
            Assert.That(framework.ClrVersion, Is.EqualTo(new Version("1.1.4322")));
        }

        [Test]
        public void ChangingRuntimeVersionUpdatesProject()
        {
            view.Runtime = "Mono";
            view.RuntimeVersion = "1.1.4322";
            RaisePropertyChangedEvent("RuntimeVersion");
            RuntimeFramework framework = model.Configs[0].RuntimeFramework;
            Assert.That(framework.Runtime, Is.EqualTo(RuntimeType.Mono));
            Assert.That(framework.ClrVersion, Is.EqualTo(new Version(1, 1, 4322)));
        }
    }
}
