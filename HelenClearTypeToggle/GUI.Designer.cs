
namespace HelenClearTypeToggle
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.openHelenGroup = new System.Windows.Forms.GroupBox();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.openHelenButton = new System.Windows.Forms.Button();
            this.versionInfoGroup = new System.Windows.Forms.GroupBox();
            this.versionInfoBox = new System.Windows.Forms.RichTextBox();
            this.chooseClearTypeGroup = new System.Windows.Forms.GroupBox();
            this.clearTypeControlOff = new System.Windows.Forms.RadioButton();
            this.clearTypeControlOn = new System.Windows.Forms.RadioButton();
            this.saveButtonGroup = new System.Windows.Forms.GroupBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.versionAuthorGithubLabel = new System.Windows.Forms.LinkLabel();
            this.openHelenGroup.SuspendLayout();
            this.versionInfoGroup.SuspendLayout();
            this.chooseClearTypeGroup.SuspendLayout();
            this.saveButtonGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // openHelenGroup
            // 
            this.openHelenGroup.Controls.Add(this.pathBox);
            this.openHelenGroup.Controls.Add(this.openHelenButton);
            this.openHelenGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openHelenGroup.Location = new System.Drawing.Point(12, 12);
            this.openHelenGroup.Name = "openHelenGroup";
            this.openHelenGroup.Size = new System.Drawing.Size(526, 60);
            this.openHelenGroup.TabIndex = 0;
            this.openHelenGroup.TabStop = false;
            this.openHelenGroup.Text = "Step 1: Select HELEN.exe";
            // 
            // pathBox
            // 
            this.pathBox.AllowDrop = true;
            this.pathBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pathBox.Location = new System.Drawing.Point(6, 26);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(473, 23);
            this.pathBox.TabIndex = 0;
            // 
            // openHelenButton
            // 
            this.openHelenButton.Location = new System.Drawing.Point(485, 24);
            this.openHelenButton.Name = "openHelenButton";
            this.openHelenButton.Size = new System.Drawing.Size(36, 27);
            this.openHelenButton.TabIndex = 1;
            this.openHelenButton.Text = "...";
            this.openHelenButton.UseVisualStyleBackColor = true;
            this.openHelenButton.Click += new System.EventHandler(this.buttonOpenHelenExe_Click);
            // 
            // versionInfoGroup
            // 
            this.versionInfoGroup.Controls.Add(this.versionInfoBox);
            this.versionInfoGroup.Location = new System.Drawing.Point(12, 86);
            this.versionInfoGroup.Name = "versionInfoGroup";
            this.versionInfoGroup.Size = new System.Drawing.Size(526, 136);
            this.versionInfoGroup.TabIndex = 1;
            this.versionInfoGroup.TabStop = false;
            this.versionInfoGroup.Text = "Step 2: HELEN.exe version information";
            this.versionInfoGroup.Enter += new System.EventHandler(this.groupVersionInformation_Enter);
            // 
            // versionInfoBox
            // 
            this.versionInfoBox.AutoWordSelection = true;
            this.versionInfoBox.BackColor = System.Drawing.SystemColors.Control;
            this.versionInfoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.versionInfoBox.Location = new System.Drawing.Point(6, 26);
            this.versionInfoBox.Name = "versionInfoBox";
            this.versionInfoBox.ReadOnly = true;
            this.versionInfoBox.Size = new System.Drawing.Size(514, 104);
            this.versionInfoBox.TabIndex = 2;
            this.versionInfoBox.Text = "Detected version: \n\nWill turn ClearType on/off: \n\nSHA1 Checksum: ";
            // 
            // chooseClearTypeGroup
            // 
            this.chooseClearTypeGroup.Controls.Add(this.clearTypeControlOff);
            this.chooseClearTypeGroup.Controls.Add(this.clearTypeControlOn);
            this.chooseClearTypeGroup.Location = new System.Drawing.Point(12, 237);
            this.chooseClearTypeGroup.Name = "chooseClearTypeGroup";
            this.chooseClearTypeGroup.Size = new System.Drawing.Size(526, 99);
            this.chooseClearTypeGroup.TabIndex = 2;
            this.chooseClearTypeGroup.TabStop = false;
            this.chooseClearTypeGroup.Text = "Step 3: Choose whether HELEN will turn ClearType on/off";
            // 
            // clearTypeControlOff
            // 
            this.clearTypeControlOff.AutoSize = true;
            this.clearTypeControlOff.Enabled = false;
            this.clearTypeControlOff.Location = new System.Drawing.Point(6, 68);
            this.clearTypeControlOff.Name = "clearTypeControlOff";
            this.clearTypeControlOff.Size = new System.Drawing.Size(409, 21);
            this.clearTypeControlOff.TabIndex = 4;
            this.clearTypeControlOff.Text = "Disabled: HELEN will never attempt to turn ClearType on/off.";
            this.clearTypeControlOff.UseVisualStyleBackColor = true;
            // 
            // clearTypeControlOn
            // 
            this.clearTypeControlOn.AutoSize = true;
            this.clearTypeControlOn.Checked = true;
            this.clearTypeControlOn.Enabled = false;
            this.clearTypeControlOn.Location = new System.Drawing.Point(6, 31);
            this.clearTypeControlOn.Name = "clearTypeControlOn";
            this.clearTypeControlOn.Size = new System.Drawing.Size(495, 21);
            this.clearTypeControlOn.TabIndex = 3;
            this.clearTypeControlOn.TabStop = true;
            this.clearTypeControlOn.Text = "Enabled: HELEN will attempt to turn ClearType on/off as required (default)";
            this.clearTypeControlOn.UseVisualStyleBackColor = true;
            // 
            // saveButtonGroup
            // 
            this.saveButtonGroup.Controls.Add(this.saveButton);
            this.saveButtonGroup.Location = new System.Drawing.Point(12, 350);
            this.saveButtonGroup.Name = "saveButtonGroup";
            this.saveButtonGroup.Size = new System.Drawing.Size(526, 78);
            this.saveButtonGroup.TabIndex = 3;
            this.saveButtonGroup.TabStop = false;
            this.saveButtonGroup.Text = "Step 4: Commit changes";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(6, 26);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(514, 46);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // versionAuthorGithubLabel
            // 
            this.versionAuthorGithubLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.versionAuthorGithubLabel.LinkArea = new System.Windows.Forms.LinkArea(28, 32);
            this.versionAuthorGithubLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.versionAuthorGithubLabel.Location = new System.Drawing.Point(2, 425);
            this.versionAuthorGithubLabel.Name = "versionAuthorGithubLabel";
            this.versionAuthorGithubLabel.Size = new System.Drawing.Size(547, 47);
            this.versionAuthorGithubLabel.TabIndex = 6;
            this.versionAuthorGithubLabel.TabStop = true;
            this.versionAuthorGithubLabel.Text = "v0.1.0 • Created by sjain • Help, Source Code & Full Credits";
            this.versionAuthorGithubLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.versionAuthorGithubLabel.UseCompatibleTextRendering = true;
            this.versionAuthorGithubLabel.UseMnemonic = false;
            this.versionAuthorGithubLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelVersionAuthorGithub_LinkClicked);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(550, 474);
            this.Controls.Add(this.versionAuthorGithubLabel);
            this.Controls.Add(this.saveButtonGroup);
            this.Controls.Add(this.chooseClearTypeGroup);
            this.Controls.Add(this.versionInfoGroup);
            this.Controls.Add(this.openHelenGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI";
            this.Text = "HELEN ClearType Control Toggler";
            this.openHelenGroup.ResumeLayout(false);
            this.openHelenGroup.PerformLayout();
            this.versionInfoGroup.ResumeLayout(false);
            this.chooseClearTypeGroup.ResumeLayout(false);
            this.chooseClearTypeGroup.PerformLayout();
            this.saveButtonGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        public System.Windows.Forms.TextBox pathBox;
        public System.Windows.Forms.Button openHelenButton;
        public System.Windows.Forms.RichTextBox versionInfoBox;
        public System.Windows.Forms.Button saveButton;
        public System.Windows.Forms.RadioButton clearTypeControlOn;
        public System.Windows.Forms.RadioButton clearTypeControlOff;
        public System.Windows.Forms.GroupBox chooseClearTypeGroup;

        private System.Windows.Forms.GroupBox openHelenGroup;
        private System.Windows.Forms.GroupBox versionInfoGroup;
        private System.Windows.Forms.GroupBox saveButtonGroup;
        private System.Windows.Forms.LinkLabel versionAuthorGithubLabel;













    }
}

