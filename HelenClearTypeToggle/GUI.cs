using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Win32;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace HelenClearTypeToggle
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void groupVersionInformation_Enter(object sender, EventArgs e)
        {

        }

        private void buttonOpenHelenExe_Click(object sender, EventArgs e)
        {

        }

        private void labelVersionAuthorGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.github.com/sjain882/HELEN-ClearType-Toggle");
        }


    }
}
