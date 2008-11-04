using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using NUnit.UiKit;
using NUnit.Util;
using NUnit.Core;

namespace NUnit.Gui
{
    public class TestAssemblyInfoForm : ScrollingTextDisplayForm
    {
        bool skipLine = false;

        protected override void OnLoad(EventArgs e)
        {
            this.Text = "Test Assemblies";
            this.TextBox.WordWrap = false;
            this.TextBox.ContentsResized += new ContentsResizedEventHandler(TextBox_ContentsResized);

            base.OnLoad(e);

            Process p = Process.GetCurrentProcess();
            int currentProcessId = p.Id;
            string currentDomainName = "";

            AppendProcessInfo( p.Id, p.MainModule.ModuleName, RuntimeFramework.CurrentFramework );

            foreach (TestAssemblyInfo info in Services.TestLoader.AssemblyInfo)
            {
                if (info.ProcessId != currentProcessId)
                {
                    AppendProcessInfo(info);
                    currentProcessId = info.ProcessId;
                }

                if (info.DomainName != currentDomainName)
                {
                    AppendDomainInfo(info);
                    currentDomainName = info.DomainName;
                }

                AppendAssemblyInfo(info);
            }
        }

        private void AppendProcessInfo(TestAssemblyInfo info)
        {
            if (skipLine) TextBox.AppendText("\r\n");
            AppendProcessInfo(info.ProcessId, info.ModuleName, info.RunnerRuntimeFramework);
            skipLine = false;
        }

        private void AppendProcessInfo( int pid, string moduleName, RuntimeFramework framework )
        {
            AppendBoldText(string.Format("{0} ( {1} )\r\n", moduleName, pid));

            TextBox.AppendText(string.Format(
                "  CLR Version: {0} ( {1} )\r\n\r\n",
                framework.Version.ToString(), 
                framework.DisplayName));
        }

        private void AppendDomainInfo(TestAssemblyInfo info)
        {
            AppendBoldText(string.Format("  {0}\r\n", info.DomainName));
            skipLine = true;
        }

        private void AppendAssemblyInfo(TestAssemblyInfo info)
        {
            AppendBoldText(
                string.Format("    {0}\r\n", Path.GetFileNameWithoutExtension(info.Name)));

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("        Path: {0}\r\n", info.Name);
            sb.AppendFormat("        Image Runtime Version: {0}\r\n", info.ImageRuntimeVersion.ToString());

            if (info.TestFrameworks != null)
            {
                string prefix = "        Uses: ";
                foreach (AssemblyName framework in info.TestFrameworks)
                {
                    sb.AppendFormat("{0}{1}\r\n", prefix, framework.FullName);
                    prefix = "              ";
                }
            }

            TextBox.AppendText(sb.ToString());
            skipLine = true;
        }

        private void AppendBoldText(string text)
        {
            TextBox.Select(TextBox.Text.Length, 0);
            TextBox.SelectionFont = new Font(TextBox.Font, FontStyle.Bold);

            TextBox.SelectedText += text;
        }

		void TextBox_ContentsResized(object sender, ContentsResizedEventArgs e)
		{
			int increase = e.NewRectangle.Width-TextBox.ClientSize.Width;
			if (increase > 0)
			{
				TextBox.Width += increase;
				this.Width += increase;
			}
		}
	}
}
