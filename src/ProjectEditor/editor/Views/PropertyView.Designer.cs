namespace NUnit.ProjectEditor
{
    partial class PropertyView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyView));
            this.domainUsageComboBox = new System.Windows.Forms.ComboBox();
            this.processModelComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.projectBaseBrowseButton = new System.Windows.Forms.Button();
            this.projectBaseTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.projectPathLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.projectTabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.runtimeVersionComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.runtimeComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.autoBinPathRadioButton = new System.Windows.Forms.RadioButton();
            this.manualBinPathRadioButton = new System.Windows.Forms.RadioButton();
            this.noBinPathRadioButton = new System.Windows.Forms.RadioButton();
            this.configBaseBrowseButton = new System.Windows.Forms.Button();
            this.privateBinPathTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.configFileTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.applicationBaseTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.assemblyTabPage = new System.Windows.Forms.TabPage();
            this.assemblyPathBrowseButton = new System.Windows.Forms.Button();
            this.assemblyPathTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.assemblyListBox = new System.Windows.Forms.ListBox();
            this.addAssemblyButton = new System.Windows.Forms.Button();
            this.removeAssemblyButton = new System.Windows.Forms.Button();
            this.editConfigsButton = new System.Windows.Forms.Button();
            this.configComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.activeConfigLabel = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.projectTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.assemblyTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // domainUsageComboBox
            // 
            this.domainUsageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.domainUsageComboBox.Items.AddRange(new object[] {
            "Default"});
            this.domainUsageComboBox.Location = new System.Drawing.Point(350, 80);
            this.domainUsageComboBox.Name = "domainUsageComboBox";
            this.domainUsageComboBox.Size = new System.Drawing.Size(100, 24);
            this.domainUsageComboBox.TabIndex = 22;
            this.domainUsageComboBox.SelectedIndexChanged += new System.EventHandler(this.domainUsageComboBox_SelectedIndexChanged);
            // 
            // processModelComboBox
            // 
            this.processModelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processModelComboBox.Items.AddRange(new object[] {
            "Default"});
            this.processModelComboBox.Location = new System.Drawing.Point(121, 80);
            this.processModelComboBox.Name = "processModelComboBox";
            this.processModelComboBox.Size = new System.Drawing.Size(106, 24);
            this.processModelComboBox.TabIndex = 21;
            this.processModelComboBox.SelectedIndexChanged += new System.EventHandler(this.processModelComboBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(238, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 16);
            this.label10.TabIndex = 20;
            this.label10.Text = "Domain Usage:";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(5, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 16);
            this.label9.TabIndex = 19;
            this.label9.Text = "Process Model:";
            // 
            // projectBaseBrowseButton
            // 
            this.projectBaseBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.projectBaseBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("projectBaseBrowseButton.Image")));
            this.projectBaseBrowseButton.Location = new System.Drawing.Point(655, 44);
            this.projectBaseBrowseButton.Name = "projectBaseBrowseButton";
            this.projectBaseBrowseButton.Size = new System.Drawing.Size(24, 20);
            this.projectBaseBrowseButton.TabIndex = 18;
            this.projectBaseBrowseButton.Click += new System.EventHandler(this.projectBaseBrowseButton_Click);
            // 
            // projectBaseTextBox
            // 
            this.projectBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectBaseTextBox.Location = new System.Drawing.Point(97, 42);
            this.projectBaseTextBox.Name = "projectBaseTextBox";
            this.projectBaseTextBox.Size = new System.Drawing.Size(537, 22);
            this.projectBaseTextBox.TabIndex = 17;
            this.projectBaseTextBox.Validated += new System.EventHandler(this.projectBaseTextBox_Validated);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(5, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 21);
            this.label8.TabIndex = 16;
            this.label8.Text = "Project Base:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(5, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 22);
            this.label5.TabIndex = 15;
            this.label5.Text = "Project Path:";
            // 
            // projectPathLabel
            // 
            this.projectPathLabel.AutoSize = true;
            this.projectPathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projectPathLabel.Location = new System.Drawing.Point(100, 5);
            this.projectPathLabel.Name = "projectPathLabel";
            this.projectPathLabel.Size = new System.Drawing.Size(0, 17);
            this.projectPathLabel.TabIndex = 23;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.projectTabControl);
            this.groupBox1.Controls.Add(this.editConfigsButton);
            this.groupBox1.Controls.Add(this.configComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(666, 366);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration Properties";
            // 
            // projectTabControl
            // 
            this.projectTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectTabControl.Controls.Add(this.generalTabPage);
            this.projectTabControl.Controls.Add(this.assemblyTabPage);
            this.projectTabControl.ItemSize = new System.Drawing.Size(49, 18);
            this.projectTabControl.Location = new System.Drawing.Point(7, 82);
            this.projectTabControl.Name = "projectTabControl";
            this.projectTabControl.SelectedIndex = 0;
            this.projectTabControl.Size = new System.Drawing.Size(651, 278);
            this.projectTabControl.TabIndex = 9;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.runtimeVersionComboBox);
            this.generalTabPage.Controls.Add(this.label11);
            this.generalTabPage.Controls.Add(this.runtimeComboBox);
            this.generalTabPage.Controls.Add(this.label7);
            this.generalTabPage.Controls.Add(this.autoBinPathRadioButton);
            this.generalTabPage.Controls.Add(this.manualBinPathRadioButton);
            this.generalTabPage.Controls.Add(this.noBinPathRadioButton);
            this.generalTabPage.Controls.Add(this.configBaseBrowseButton);
            this.generalTabPage.Controls.Add(this.privateBinPathTextBox);
            this.generalTabPage.Controls.Add(this.label6);
            this.generalTabPage.Controls.Add(this.configFileTextBox);
            this.generalTabPage.Controls.Add(this.label4);
            this.generalTabPage.Controls.Add(this.applicationBaseTextBox);
            this.generalTabPage.Controls.Add(this.label3);
            this.generalTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Size = new System.Drawing.Size(643, 252);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            // 
            // runtimeVersionComboBox
            // 
            this.runtimeVersionComboBox.Location = new System.Drawing.Point(320, 16);
            this.runtimeVersionComboBox.Name = "runtimeVersionComboBox";
            this.runtimeVersionComboBox.Size = new System.Drawing.Size(101, 24);
            this.runtimeVersionComboBox.TabIndex = 14;
            this.runtimeVersionComboBox.Validated += new System.EventHandler(this.runtimeVersionComboBox_Validated);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(192, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 16);
            this.label11.TabIndex = 13;
            this.label11.Text = "Runtime Version";
            // 
            // runtimeComboBox
            // 
            this.runtimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runtimeComboBox.Items.AddRange(new object[] {
            "Any"});
            this.runtimeComboBox.Location = new System.Drawing.Point(87, 16);
            this.runtimeComboBox.Name = "runtimeComboBox";
            this.runtimeComboBox.Size = new System.Drawing.Size(81, 24);
            this.runtimeComboBox.TabIndex = 12;
            this.runtimeComboBox.SelectedIndexChanged += new System.EventHandler(this.runtimeComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(13, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 16);
            this.label7.TabIndex = 11;
            this.label7.Text = "Runtime:";
            // 
            // autoBinPathRadioButton
            // 
            this.autoBinPathRadioButton.Location = new System.Drawing.Point(24, 154);
            this.autoBinPathRadioButton.Name = "autoBinPathRadioButton";
            this.autoBinPathRadioButton.Size = new System.Drawing.Size(273, 21);
            this.autoBinPathRadioButton.TabIndex = 10;
            this.autoBinPathRadioButton.Text = "Use automatically generated path";
            this.autoBinPathRadioButton.CheckedChanged += new System.EventHandler(this.autoBinPathRadioButton_CheckedChanged);
            // 
            // manualBinPathRadioButton
            // 
            this.manualBinPathRadioButton.Location = new System.Drawing.Point(24, 186);
            this.manualBinPathRadioButton.Name = "manualBinPathRadioButton";
            this.manualBinPathRadioButton.Size = new System.Drawing.Size(101, 20);
            this.manualBinPathRadioButton.TabIndex = 9;
            this.manualBinPathRadioButton.Text = "Use this path:";
            this.manualBinPathRadioButton.CheckedChanged += new System.EventHandler(this.manualBinPathRadioButton_CheckedChanged);
            // 
            // noBinPathRadioButton
            // 
            this.noBinPathRadioButton.Location = new System.Drawing.Point(24, 218);
            this.noBinPathRadioButton.Name = "noBinPathRadioButton";
            this.noBinPathRadioButton.Size = new System.Drawing.Size(353, 21);
            this.noBinPathRadioButton.TabIndex = 8;
            this.noBinPathRadioButton.Text = "None - or specified in Configuration File";
            this.noBinPathRadioButton.CheckedChanged += new System.EventHandler(this.noBinPathRadioButton_CheckedChanged);
            // 
            // configBaseBrowseButton
            // 
            this.configBaseBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configBaseBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("configBaseBrowseButton.Image")));
            this.configBaseBrowseButton.Location = new System.Drawing.Point(606, 56);
            this.configBaseBrowseButton.Name = "configBaseBrowseButton";
            this.configBaseBrowseButton.Size = new System.Drawing.Size(20, 20);
            this.configBaseBrowseButton.TabIndex = 7;
            this.configBaseBrowseButton.Click += new System.EventHandler(this.configBaseBrowseButton_Click);
            // 
            // privateBinPathTextBox
            // 
            this.privateBinPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.privateBinPathTextBox.Location = new System.Drawing.Point(144, 186);
            this.privateBinPathTextBox.Name = "privateBinPathTextBox";
            this.privateBinPathTextBox.Size = new System.Drawing.Size(485, 22);
            this.privateBinPathTextBox.TabIndex = 5;
            this.privateBinPathTextBox.Validated += new System.EventHandler(this.privateBinPathTextBox_Validated);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "PrivateBinPath:";
            // 
            // configFileTextBox
            // 
            this.configFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configFileTextBox.Location = new System.Drawing.Point(168, 96);
            this.configFileTextBox.Name = "configFileTextBox";
            this.configFileTextBox.Size = new System.Drawing.Size(461, 22);
            this.configFileTextBox.TabIndex = 3;
            this.configFileTextBox.Validated += new System.EventHandler(this.configFileTextBox_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Configuration File Name:";
            // 
            // applicationBaseTextBox
            // 
            this.applicationBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.applicationBaseTextBox.Location = new System.Drawing.Point(128, 56);
            this.applicationBaseTextBox.Name = "applicationBaseTextBox";
            this.applicationBaseTextBox.Size = new System.Drawing.Size(469, 22);
            this.applicationBaseTextBox.TabIndex = 1;
            this.applicationBaseTextBox.Validated += new System.EventHandler(this.applicationBaseTextBox_Validated);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "ApplicationBase:";
            // 
            // assemblyTabPage
            // 
            this.assemblyTabPage.Controls.Add(this.assemblyPathBrowseButton);
            this.assemblyTabPage.Controls.Add(this.assemblyPathTextBox);
            this.assemblyTabPage.Controls.Add(this.label2);
            this.assemblyTabPage.Controls.Add(this.assemblyListBox);
            this.assemblyTabPage.Controls.Add(this.addAssemblyButton);
            this.assemblyTabPage.Controls.Add(this.removeAssemblyButton);
            this.assemblyTabPage.Location = new System.Drawing.Point(4, 22);
            this.assemblyTabPage.Name = "assemblyTabPage";
            this.assemblyTabPage.Size = new System.Drawing.Size(643, 252);
            this.assemblyTabPage.TabIndex = 1;
            this.assemblyTabPage.Text = "Assemblies";
            this.assemblyTabPage.Visible = false;
            // 
            // assemblyPathBrowseButton
            // 
            this.assemblyPathBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assemblyPathBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("assemblyPathBrowseButton.Image")));
            this.assemblyPathBrowseButton.Location = new System.Drawing.Point(570, 192);
            this.assemblyPathBrowseButton.Name = "assemblyPathBrowseButton";
            this.assemblyPathBrowseButton.Size = new System.Drawing.Size(20, 20);
            this.assemblyPathBrowseButton.TabIndex = 11;
            this.assemblyPathBrowseButton.Click += new System.EventHandler(this.assemblyPathBrowseButton_Click);
            // 
            // assemblyPathTextBox
            // 
            this.assemblyPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.assemblyPathTextBox.Location = new System.Drawing.Point(13, 192);
            this.assemblyPathTextBox.Name = "assemblyPathTextBox";
            this.assemblyPathTextBox.Size = new System.Drawing.Size(539, 22);
            this.assemblyPathTextBox.TabIndex = 8;
            this.assemblyPathTextBox.Validated += new System.EventHandler(this.assemblyPathTextBox_Validated);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "Assembly Path:";
            // 
            // assemblyListBox
            // 
            this.assemblyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.assemblyListBox.ItemHeight = 16;
            this.assemblyListBox.Location = new System.Drawing.Point(13, 24);
            this.assemblyListBox.Name = "assemblyListBox";
            this.assemblyListBox.Size = new System.Drawing.Size(492, 132);
            this.assemblyListBox.TabIndex = 6;
            this.assemblyListBox.SelectedIndexChanged += new System.EventHandler(this.assemblyListBox_SelectedIndexChanged);
            // 
            // addAssemblyButton
            // 
            this.addAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addAssemblyButton.Location = new System.Drawing.Point(513, 21);
            this.addAssemblyButton.Name = "addAssemblyButton";
            this.addAssemblyButton.Size = new System.Drawing.Size(78, 33);
            this.addAssemblyButton.TabIndex = 2;
            this.addAssemblyButton.Text = "&Add...";
            this.addAssemblyButton.Click += new System.EventHandler(this.addAssemblyButton_Click);
            // 
            // removeAssemblyButton
            // 
            this.removeAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeAssemblyButton.Location = new System.Drawing.Point(513, 60);
            this.removeAssemblyButton.Name = "removeAssemblyButton";
            this.removeAssemblyButton.Size = new System.Drawing.Size(78, 28);
            this.removeAssemblyButton.TabIndex = 5;
            this.removeAssemblyButton.Text = "&Remove";
            this.removeAssemblyButton.Click += new System.EventHandler(this.removeAssemblyButton_Click);
            // 
            // editConfigsButton
            // 
            this.editConfigsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editConfigsButton.Location = new System.Drawing.Point(505, 33);
            this.editConfigsButton.Name = "editConfigsButton";
            this.editConfigsButton.Size = new System.Drawing.Size(147, 26);
            this.editConfigsButton.TabIndex = 8;
            this.editConfigsButton.Text = "&Edit Configs...";
            this.editConfigsButton.Click += new System.EventHandler(this.editConfigsButton_Click);
            // 
            // configComboBox
            // 
            this.configComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configComboBox.ItemHeight = 16;
            this.configComboBox.Location = new System.Drawing.Point(108, 34);
            this.configComboBox.Name = "configComboBox";
            this.configComboBox.Size = new System.Drawing.Size(391, 24);
            this.configComboBox.TabIndex = 7;
            this.configComboBox.SelectedIndexChanged += new System.EventHandler(this.configComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Configuration:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(477, 85);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 17);
            this.label12.TabIndex = 25;
            this.label12.Text = "Active Config:";
            // 
            // activeConfigLabel
            // 
            this.activeConfigLabel.AutoSize = true;
            this.activeConfigLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeConfigLabel.Location = new System.Drawing.Point(577, 84);
            this.activeConfigLabel.Name = "activeConfigLabel";
            this.activeConfigLabel.Size = new System.Drawing.Size(0, 17);
            this.activeConfigLabel.TabIndex = 26;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // PropertyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.activeConfigLabel);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.projectPathLabel);
            this.Controls.Add(this.domainUsageComboBox);
            this.Controls.Add(this.processModelComboBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.projectBaseBrowseButton);
            this.Controls.Add(this.projectBaseTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Name = "PropertyView";
            this.Size = new System.Drawing.Size(701, 496);
            this.groupBox1.ResumeLayout(false);
            this.projectTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.generalTabPage.PerformLayout();
            this.assemblyTabPage.ResumeLayout(false);
            this.assemblyTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox domainUsageComboBox;
        private System.Windows.Forms.ComboBox processModelComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button projectBaseBrowseButton;
        private System.Windows.Forms.TextBox projectBaseTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label projectPathLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl projectTabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.ComboBox runtimeVersionComboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox runtimeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton autoBinPathRadioButton;
        private System.Windows.Forms.RadioButton manualBinPathRadioButton;
        private System.Windows.Forms.RadioButton noBinPathRadioButton;
        private System.Windows.Forms.Button configBaseBrowseButton;
        private System.Windows.Forms.TextBox privateBinPathTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox configFileTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox applicationBaseTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage assemblyTabPage;
        private System.Windows.Forms.Button assemblyPathBrowseButton;
        private System.Windows.Forms.TextBox assemblyPathTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox assemblyListBox;
        private System.Windows.Forms.Button addAssemblyButton;
        private System.Windows.Forms.Button removeAssemblyButton;
        private System.Windows.Forms.Button editConfigsButton;
        private System.Windows.Forms.ComboBox configComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label activeConfigLabel;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}
