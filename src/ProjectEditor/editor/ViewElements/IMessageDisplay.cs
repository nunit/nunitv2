using System;
using System.Windows.Forms;

namespace NUnit.ProjectEditor.ViewElements
{
    public interface IMessageDisplay
    {
        void Error(string message);

        bool AskYesNoQuestion(string question);
    }
}
