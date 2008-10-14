using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using NUnit.UiKit;
using NUnit.Util;
using NUnit.Core;

namespace NUnit.Gui
{
    public class TestAssemblyInfoForm : ScrollingTextDisplayForm
    {
        protected override void OnLoad(EventArgs e)
        {
            Message.Text = "Loaded Test Assemblies:";

            base.OnLoad(e);

            IList infoList = Services.TestLoader.AssemblyInfo;

            if (infoList == null || infoList.Count == 0)
                TextBox.Text = "No assemblies are loaded.";
            else
            {
                foreach (TestAssemblyInfo info in infoList)
                {
                    RuntimeFramework runtime = info.RunnerRuntimeFramework;
                    Version imageVersion = info.RunnerRuntimeFramework.Version;

                    TextBox.Select(TextBox.Text.Length, 0);
                    TextBox.SelectionFont = new Font(TextBox.Font, FontStyle.Bold);

                    TextBox.SelectedText +=
                        string.Format("{0}\r\n", Path.GetFileNameWithoutExtension(info.Name));

                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("  Path: {0}\r\n", info.Name);
                    sb.AppendFormat("  Image Runtime Version: {0})\r\n", imageVersion.ToString());
                    sb.AppendFormat("  Running Under: {0} ( {1} )\r\n",
                        runtime.Version.ToString(), runtime.GetDisplayName());
                    sb.AppendFormat("  Process: {0}  {1}\r\n", info.ProcessId, info.ModuleName);
                    sb.AppendFormat("  Domain: {0}\r\n", info.DomainName);
                    if (info.TestFrameworks != null)
                    {
                        string prefix = "  Uses: ";
                        foreach (AssemblyName framework in info.TestFrameworks)
                        {
                            sb.AppendFormat("{0}{1}\r\n", prefix, framework.FullName);
                            prefix = "        ";
                        }
                    }
                    sb.Append("\r\n");
                    TextBox.AppendText(sb.ToString());
                }
            }
        }
    }
}
