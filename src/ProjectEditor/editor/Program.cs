// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set up main editor triad
            ProjectDocument doc = new ProjectDocument();
            MainForm view = new MainForm();
            new MainPresenter(doc, view);

            if (args.Length > 0)
                doc.OpenProject(args[0]);

            Application.Run(view);
        }
    }
}
