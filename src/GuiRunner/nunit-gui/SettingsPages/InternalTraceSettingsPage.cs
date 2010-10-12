using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NUnit.Core;

namespace NUnit.Gui.SettingsPages
{
    public partial class InternalTraceSettingsPage : NUnit.UiKit.SettingsPage
    {
        public InternalTraceSettingsPage(string key) : base(key)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            traceLevelComboBox.SelectedIndex = (int)(InternalTraceLevel)settings.GetSetting("Options.InternalTraceLevel", InternalTraceLevel.Default);
            logDirectoryLabel.Text = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "logs");
        }

        public override void ApplySettings()
        {
           settings.SaveSetting("Options.InternalTraceLevel", (InternalTraceLevel)traceLevelComboBox.SelectedIndex);
        }
    }
}
