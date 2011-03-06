// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using NUnit.Core;

namespace NUnit.Gui.SettingsPages
{
    public partial class RuntimeSelectionSettingsPage : NUnit.UiKit.SettingsPage
    {
        private static readonly string RUNTIME_SELECTION_ENABLED =
            "Options.TestLoader.RuntimeSelectionEnabled";
        private static readonly string NET11_SUPPORT_ENABLED =
            "Options.TestLoader.Net-1.1.Support";
        private static readonly string NET11_BIN_DIRECTORY =
            "Options.TestLoader.Net-1.1.BinDirectory";

        public RuntimeSelectionSettingsPage(string key) : base(key)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            runtimeSelectionCheckBox.Checked = settings.GetSetting(RUNTIME_SELECTION_ENABLED, true);
            net11SupportCheckBox.Checked = settings.GetSetting(NET11_SUPPORT_ENABLED, false);
            net11BinDirectoryTextBox.Text = settings.GetSetting(NET11_BIN_DIRECTORY) as string;
            net11BinDirectoryTextBox.Enabled = net11SupportCheckBox.Checked;
        }

        public override void ApplySettings()
        {
            settings.SaveSetting(RUNTIME_SELECTION_ENABLED, runtimeSelectionCheckBox.Checked);
            settings.SaveSetting(NET11_SUPPORT_ENABLED, net11SupportCheckBox.Checked);
            settings.SaveSetting(NET11_BIN_DIRECTORY, net11BinDirectoryTextBox.Text);
        }

        private void runtimeSelectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            net11SupportCheckBox.Enabled = runtimeSelectionCheckBox.Checked;
        }

        private void net11SupportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            net11BinDirectoryTextBox.Enabled = net11SupportCheckBox.Checked;
        }
    }
}
