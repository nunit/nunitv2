namespace NUnit.Gui.SettingsPages
{
    partial class RuntimeSelectionSettingsPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.runtimeSelectionCheckBox = new System.Windows.Forms.CheckBox();
            this.net11BinDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.net11SupportCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Runtime Selection";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(121, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(328, 8);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            // 
            // runtimeSelectionCheckBox
            // 
            this.runtimeSelectionCheckBox.AutoSize = true;
            this.runtimeSelectionCheckBox.Location = new System.Drawing.Point(35, 33);
            this.runtimeSelectionCheckBox.Name = "runtimeSelectionCheckBox";
            this.runtimeSelectionCheckBox.Size = new System.Drawing.Size(337, 17);
            this.runtimeSelectionCheckBox.TabIndex = 15;
            this.runtimeSelectionCheckBox.Text = "Select runtime version based on target framework of test assembly";
            this.runtimeSelectionCheckBox.UseVisualStyleBackColor = true;
            this.runtimeSelectionCheckBox.CheckedChanged += new System.EventHandler(this.runtimeSelectionCheckBox_CheckedChanged);
            // 
            // net11BinDirectoryTextBox
            // 
            this.net11BinDirectoryTextBox.Location = new System.Drawing.Point(52, 115);
            this.net11BinDirectoryTextBox.Name = "net11BinDirectoryTextBox";
            this.net11BinDirectoryTextBox.Size = new System.Drawing.Size(335, 20);
            this.net11BinDirectoryTextBox.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Path to .NET 1.x NUnit binaries";
            // 
            // net11SupportCheckBox
            // 
            this.net11SupportCheckBox.AutoSize = true;
            this.net11SupportCheckBox.Location = new System.Drawing.Point(35, 69);
            this.net11SupportCheckBox.Name = "net11SupportCheckBox";
            this.net11SupportCheckBox.Size = new System.Drawing.Size(169, 17);
            this.net11SupportCheckBox.TabIndex = 18;
            this.net11SupportCheckBox.Text = "Enable .NET 1.0 / 1.1 support";
            this.net11SupportCheckBox.UseVisualStyleBackColor = true;
            this.net11SupportCheckBox.CheckedChanged += new System.EventHandler(this.net11SupportCheckBox_CheckedChanged);
            // 
            // RuntimeSelectionSettingsPage
            // 
            this.Controls.Add(this.net11SupportCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.net11BinDirectoryTextBox);
            this.Controls.Add(this.runtimeSelectionCheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Name = "RuntimeSelectionSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox runtimeSelectionCheckBox;
        private System.Windows.Forms.TextBox net11BinDirectoryTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox net11SupportCheckBox;
    }
}
