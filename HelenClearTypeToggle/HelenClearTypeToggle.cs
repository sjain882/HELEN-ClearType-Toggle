using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HelenClearTypeToggle
{
    public class HelenClearTypeToggle
    {

        // Initialisation
        public GUI form;
        private Timer pathBoxTextChangedTimer;
        private const Byte SPI_GETCLEARTYPE_ARG = 0x4A;
        private const Byte SPI_SETCLEARTYPE_ARG = 0x4B;
        private static bool validPathDetected;
        private static bool willHaveClearTypeControl;
        private static bool isGUIActive;
        private static Version helenExeVersion;
        private static string helenExeURL;

        // Public access for helenExeURL
        public string HelenExeURL
        {
            get { return helenExeURL; }
            set { helenExeURL = value;  }
        }

        // Supported versions
        private enum Version
        {
            vUnknown = -1,
            v30300,
            v31002,
            v31206,
            v30300_Patched,
            v31002_Patched,
            v31206_Patched
        };

        // Corresponds to the 'Version' enum above
        private static string[] VersionChecksums =
        {
            "4249D5624FC24434F6A5BD837F560AF609E21AB2",
            "34A1885B394F2B6DA7511C7B21DE8B8B280196BE",
            "C484206174A1CE451BBFD14BA0142BF8421EF788",
            "2F9E0591428115C80A717DE3214A79F06787534C",
            "2D334C78A59F1C3E0ADBA75EE1F60293A83649ED",
            "9A828DAF295544BC190A678404A5C722D36A5109"
        };

        // Corresponds to the 'Version' enum above
        private static string[] VersionStrings =
        {
            "v3.03.00",
            "v3.10.02",
            "v3.12.06",
            "v3.03.00",
            "v3.10.02",
            "v3.12.06"
        };

        // These must correspond to the `Version` enum above
        private static bool[] VersionHasClearTypeControl =
        {
            true,
            true,
            true,
            false,
            false,
            false
        };

        // Available fonts for "Version information" RichTextBox
        private enum FontStyle
        {
            BoldGreen,
            BoldRed,
            RegularAuto
        };


        /* Constructor
         * if launchGUI true, GUI will launch
         * if launchGUI false, GUI will not launch (CLI mode) */
        public HelenClearTypeToggle(bool launchGUI)
        {

            // Initialisation
            helenExeURL = "";
            willHaveClearTypeControl = true;
            helenExeVersion = Version.vUnknown;
            isGUIActive = false;
            validPathDetected = false;

            if (launchGUI)
            {
                // Initialise GUI
                isGUIActive = true;
                form = new GUI();

                // Event handlers
                form.pathBox.TextChanged +=
                              new System.EventHandler(this.pathBox_TextChanged);

                form.openHelenButton.Click +=
                             new System.EventHandler(this.OpenHelenExeByDialog);

                form.clearTypeControlOn.CheckedChanged +=
                       new System.EventHandler(RadioButtonClearTypeOn_Selected);

                form.clearTypeControlOff.CheckedChanged +=
                      new System.EventHandler(RadioButtonClearTypeOff_Selected);

                form.saveButton.Click +=
                   new System.EventHandler(delegate (object sender, EventArgs e)
                                           { Patch(willHaveClearTypeControl);});

                // Prevent maximising
                form.MaximizeBox = false;

                // Timer for GUI to register a change in the path box's contents
                pathBoxTextChangedTimer = new Timer();
                pathBoxTextChangedTimer.Tick += new EventHandler(pathBoxTimer_Tick);
                pathBoxTextChangedTimer.Interval = 1000;

            }
        }


        // Code executed when app registers a change in the path box's contents
        private void pathBoxTimer_Tick(object sender, EventArgs e)
        {

            pathBoxTextChangedTimer.Stop();

            if (this.ValidateHelenEXEPath(form.pathBox.Text, true))
            {
                helenExeURL = form.pathBox.Text;
            }

            UpdateVersionInfo();

        }


        /* When path box is changed, trigger the timer in order to start
         * a ratelimited path changed event. */
        private void pathBox_TextChanged(object sender, EventArgs e)
        {
            pathBoxTextChangedTimer.Stop();
            pathBoxTextChangedTimer.Start();
        }


        // ClearType behaviour radio button selection events
        private void RadioButtonClearTypeOn_Selected(object sender, EventArgs e)
        {
            willHaveClearTypeControl = true;
        }
        private void RadioButtonClearTypeOff_Selected(object sender, EventArgs e)
        {
            willHaveClearTypeControl = false;
        }


        // Triggered by [...] button next to path box.
        // Sets helenExeUrl to the full path URL returned by the OpenFileDialog.
        private void OpenHelenExeByDialog(object sender, EventArgs e)
        {

            /* "using" so that the file handle is closed as soon as this
             * block of code is completed */
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "Please select your HELEN executable file (*.exe)";
                fileDialog.Filter = "HELEN Executable|HELEN.exe";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {

                    helenExeURL = Path.GetFullPath(fileDialog.FileName);
                    form.pathBox.Text = helenExeURL;

                    if (ValidateHelenEXEPath(helenExeURL, false))
                    {
                        UpdateVersionInfo();
                    }
                }
            }
        }


        /* Sets the font style of the version information RichTextBox 
         * to the specified FontStyle */
        private void SetVersionInfoFont(FontStyle fontStyle)
        {

            switch (fontStyle)
            {
                case FontStyle.BoldGreen:
                    form.versionInfoBox.SelectionFont =
                                        new Font(form.versionInfoBox.Font,
                                                 System.Drawing.FontStyle.Bold);

                    form.versionInfoBox.SelectionColor = Color.Green;
                    break;

                case FontStyle.BoldRed:
                    form.versionInfoBox.SelectionFont =
                                        new Font(form.versionInfoBox.Font,
                                                 System.Drawing.FontStyle.Bold);
                    form.versionInfoBox.SelectionColor = Color.Red;
                    break;

                case FontStyle.RegularAuto:
                    form.versionInfoBox.SelectionFont =
                                        new Font(form.versionInfoBox.Font,
                                              System.Drawing.FontStyle.Regular);
                    form.versionInfoBox.SelectionColor =
                                         System.Drawing.SystemColors.WindowText;
                    break;
            }
        }


        /* Updates the HELEN version information, setting relevant global
         * variables, and updating the version information RichTextBox
         * if the GUI is active. */
        public void UpdateVersionInfo()
        {

            // Initialisation
            int versionIndex = -1;
            string sha1Checksum = "";
            bool hasClearTypeControl = true;

            /* If a valid path to an openable executable was specified,
             * calculate the checksum and get the version from that */
            if (validPathDetected)
            {
                sha1Checksum = CalculateSHA1Checksum();
                versionIndex = Array.IndexOf(VersionChecksums, sha1Checksum);
                helenExeVersion = (Version)versionIndex;
            }

            // GUI logic - update the GUI so it reflects the version information
            if (isGUIActive)
            {
                /* Make controls available if exe is patchable, or unavailable
                 * if exe is not patchable (version is unknown) */

                // If valid exe was detected but its version couldn't be found:
                if (helenExeVersion == Version.vUnknown || !validPathDetected)
                {
                    form.clearTypeControlOn.Enabled = false;
                    form.clearTypeControlOff.Enabled = false;
                    form.saveButton.Enabled = false;
                }
                // If valid exe was detected and its version could be found:
                else
                {
                    form.clearTypeControlOn.Enabled = true;
                    form.clearTypeControlOff.Enabled = true;
                    form.saveButton.Enabled = true;

                    /* Determine if the HELEN executable currently has control
                     * over ClearType */
                    hasClearTypeControl = VersionHasClearTypeControl[versionIndex];

                    /* Update the status of the ClearType control radio buttons
                     * to reflect this */
                    if (hasClearTypeControl)
                    {
                        form.clearTypeControlOn.Checked = true;
                        form.clearTypeControlOff.Checked = false;
                    }
                    else
                    {
                        form.clearTypeControlOn.Checked = false;
                        form.clearTypeControlOff.Checked = true;
                    }
                }

                /* Update ("re-write") the version information RichTextBox.
                 * To ensure that it actually retains font settings, etc,
                 * We must empty its text and THEN use AppendText().
                 * https://stackoverflow.com/a/25708977/12948636 */
                form.versionInfoBox.Text = "";
                
                // Label
                SetVersionInfoFont(FontStyle.RegularAuto);
                form.versionInfoBox.AppendText("Detected version: ");

                // Set version number
                if (!validPathDetected)
                {
                    form.versionInfoBox.AppendText("\n\n");
                }
                else if (helenExeVersion == Version.vUnknown && validPathDetected)
                {
                    SetVersionInfoFont(FontStyle.BoldRed);
                    form.versionInfoBox.AppendText("Unknown\n\n");
                }
                else if (helenExeVersion != Version.vUnknown)
                {
                    SetVersionInfoFont(FontStyle.BoldGreen);
                    form.versionInfoBox.AppendText(VersionStrings[versionIndex]
                                                   + "\n\n");
                }

                // Label
                SetVersionInfoFont(FontStyle.RegularAuto);
                form.versionInfoBox.AppendText("Will turn ClearType on/off: ");

                // Set ClearType control status
                // WARNING: The order of these conditions is significant!
                if (helenExeVersion == Version.vUnknown && validPathDetected)
                {
                    SetVersionInfoFont(FontStyle.BoldRed);
                    form.versionInfoBox.AppendText("Unknown\n\n");
                }
                else if (hasClearTypeControl && validPathDetected)
                {
                    SetVersionInfoFont(FontStyle.BoldGreen);
                    form.versionInfoBox.AppendText("Yes\n\n");
                }
                else if (!hasClearTypeControl)
                {
                    SetVersionInfoFont(FontStyle.BoldRed);
                    form.versionInfoBox.AppendText("No\n\n");
                }
                else if (!validPathDetected)
                {
                    form.versionInfoBox.AppendText("\n\n");
                }

                // Label
                SetVersionInfoFont(FontStyle.RegularAuto);
                form.versionInfoBox.AppendText("SHA1 Checksum:\n");

                // Set SHA1 checksum
                if (validPathDetected) form.versionInfoBox.AppendText(sha1Checksum);
            }
        }


        /* Checks that the specified path to the HELEN executable actually points
         * to an executable file that exists & can be opened at current 
         * user permission level. */
        public bool ValidateHelenEXEPath(string pathToValidate, bool silent)
        {

            try
            {
                using (File.Open(pathToValidate, FileMode.Open,
                                 FileAccess.ReadWrite)) { };
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    // Display the error in a way that is helpful to the user
                    MessageBox.Show(
                        "Failed to open the HELEN executable: " + e.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                validPathDetected = false;
                return false;
            }

            validPathDetected = true;
            return true;
        }


        /* Calculates the SHA1 checksum of a file and returns it as a String.
         * Only requires a URL to the file as a string, currently hardcoded
         * to the global variable helenExeURL. 
         * Code snippet entirely by MattKC (https://github.com/itsmattkc)
         * for LEGO Island Rebuilder.NET:
         * (https://github.com/itsmattkc/LEGOIslandRebuilder/tree/net) */
        private static String CalculateSHA1Checksum()
        {
            string finalHash = "";

            using (FileStream fs = new FileStream(helenExeURL,
                                                  FileMode.Open,
                                                  FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(bs);
                StringBuilder formatted = new StringBuilder(2 * hash.Length);
                foreach (byte b in hash)
                {
                    formatted.AppendFormat("{0:X2}", b);
                }

                finalHash = formatted.ToString();
                return finalHash;
            }
        }


        /* Writes a given byte to a given position in a given FileStream. 
         * Code snippet entirely by MattKC (https://github.com/itsmattkc)
         * for LEGO Island Rebuilder.NET:
         * (https://github.com/itsmattkc/LEGOIslandRebuilder/tree/net) */
        private void WriteByte(FileStream fs, byte b, long pos = -1)
        {
            if (pos > -1) fs.Position = pos;
            fs.WriteByte(b);
        }


        /* Patches the HELEN executable and creates a backup copy of the original
         * build ONLY (i.e., a build that has never been patched
         * Only requires the URL to the file as a string (currently hardcoded
         * to the global variable helenExeURL) and a boolean which determines
         * the nature of the patch (willHaveClearTypeControl) */
        public bool Patch(bool willHaveClearTypeControl)
        {

            // If unpatchable, return before attempting anything
            if (helenExeVersion == Version.vUnknown) return false;

            // Initialisation
            bool backupWillBeCreated = false;
            Byte clearTypeArgToWrite;

            // Logic determining the nature of the patch
            if (willHaveClearTypeControl)
            {
                clearTypeArgToWrite = SPI_SETCLEARTYPE_ARG;
            }
            else
            {
                clearTypeArgToWrite = SPI_GETCLEARTYPE_ARG;
            }

            try
            {

                /* Creates a backup copy of the executable only if it doesn't
                 * exist already - assuming no other patchers exist that create
                 * a backup with the same name, this should mean that this is
                 * a completely original copy of HELEN before any modification. */
                if (!File.Exists(Path.GetDirectoryName(helenExeURL) +
                                                       "\\HELEN.exe.bak"))
                {
                    backupWillBeCreated = true;
                    File.Copy(@helenExeURL, @helenExeURL + @".bak");
                }

                /* Open an executable with the specified path in a way that we
                 * can read and write to it. The read ensures it exists at all,
                 * and the write ensures we can patch it. This tests that the
                 * file exists and we have sufficient permissions. If not, the
                 * "catch" block will be jumped to from here. */
                using (FileStream helenExe = File.Open(helenExeURL,
                                                       FileMode.Open,
                                                       FileAccess.ReadWrite))
                {
                    // Initialisation
                    long clearTypeArgOffset1 = 0x000000;
                    long clearTypeArgOffset2 = 0x000000;
                    long clearTypeArgOffset3 = 0x000000;
                    long clearTypeArgOffset4 = 0x000000;
                    long clearTypeArgOffset5 = 0x000000;

                    // WARNING: This switch case uses fallthroughs / cascades!
                    // 1. Set the offsets based on the executable version
                    switch (helenExeVersion)
                    {
                        case Version.v30300:
                        case Version.v30300_Patched:
                            clearTypeArgOffset1 = 0x26D058;
                            clearTypeArgOffset2 = 0x2740D5;
                            clearTypeArgOffset3 = 0x341C22;
                            clearTypeArgOffset4 = 0x341C8F;
                            clearTypeArgOffset5 = 0x345CDF;
                            break;

                        case Version.v31002:
                        case Version.v31002_Patched:
                            clearTypeArgOffset1 = 0x1DA8F2;
                            clearTypeArgOffset2 = 0x1DA95F;
                            clearTypeArgOffset3 = 0x1DE9AF;
                            clearTypeArgOffset4 = 0x33FF37;
                            clearTypeArgOffset5 = 0x345B15;
                            break;

                        case Version.v31206:
                        case Version.v31206_Patched:
                            clearTypeArgOffset1 = 0x3C1D27;
                            clearTypeArgOffset2 = 0x1DF572;
                            clearTypeArgOffset3 = 0x1DF5DF;
                            clearTypeArgOffset4 = 0x1E362F;
                            clearTypeArgOffset5 = 0x3C79F5;
                            break;

                        default:
                            break;
                    }

                    // 2. Perform the patch
                    WriteByte(helenExe, clearTypeArgToWrite, clearTypeArgOffset1);
                    WriteByte(helenExe, clearTypeArgToWrite, clearTypeArgOffset2);
                    WriteByte(helenExe, clearTypeArgToWrite, clearTypeArgOffset3);
                    WriteByte(helenExe, clearTypeArgToWrite, clearTypeArgOffset4);
                    WriteByte(helenExe, clearTypeArgToWrite, clearTypeArgOffset5);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Patch failed with error: " + e.Message,
                                "HELEN ClearType Control Toggler",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            // If a backup was created, inform the user
            if (backupWillBeCreated)
            {
                MessageBox.Show(
                    "Patch successful!\n\n" +
                    "Since this was the first time ever patching HELEN," +
                    " a backup copy was created. This will not be" +
                    " overwritten by future patches.\n\n" +
                    "If you would like to restore it, simply delete HELEN.exe" +
                    " and rename HELEN.exe.bak to HELEN.exe.",
                    "HELEN ClearType Control Toggler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Patch successful!",
                    "HELEN ClearType Control Toggler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            /* Update the version information so the application is aware of the
             * change it just made (mainly for GUI purposes) */
            UpdateVersionInfo();

            return true;
        }
    }
}
