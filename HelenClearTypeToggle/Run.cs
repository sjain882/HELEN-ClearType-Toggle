using System;
using System.Windows.Forms;

namespace HelenClearTypeToggle
{
    class Run
    {
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialisation
            bool argsProvided = false;
            bool pathSuccessfullySet = false;
            bool pathArgProvided = false;
            bool enableArgProvided = false;
            bool disableArgProvided = false;
            string[] arguments = Environment.GetCommandLineArgs();

            /* Even if no actual arguments (like "--patch") are specified,
             * the first argument is always the executable itself.
             * e.g., in cmd: "C:\Files> HelenClearTypeToggle" ->
             *                     first and only arg is HelenClearTypeToggle
             * e.g., launching from explorer ->
             *        first and only arg is C:\Files\HelenClearTypeToggle.exe
             *
             * This separates all provided arguments by a space (' ') */
            if (arguments.Length > 1) argsProvided = true;

            // Start an instance of the patcher
            HelenClearTypeToggle instance = new HelenClearTypeToggle(!argsProvided);

            // Parse arguments
            if (argsProvided)
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    // WARNING: This switch case uses fallthroughs / cascades!
                    switch (arguments[i])
                    {

                        case "--path":
                        case "-p":
                            pathArgProvided = true;
                            string path = "";

                            /* If this arg was specified, the arg after this
                             * should be the path itself... */
                            if (i + 1 < arguments.Length)
                            {
                                path = arguments[i + 1];
                            }
                            // ...if that was not the case, then:
                            else
                            {
                                MessageBox.Show("No path specified!",
                                                "HELEN ClearType Control Toggler",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                                Environment.Exit(0);
                            }

                            // If the specified path was valid, then set it
                            if (instance.ValidateHelenEXEPath(path, false))
                            {
                                instance.HelenExeURL = path;
                                if (instance.HelenExeURL.Length > 0)
                                                    pathSuccessfullySet = true;
                            }

                            break;

                        case "--enable":
                        case "-e":
                            /* We don't perform the patch right here, but track
                             * whether it's been specified or not instead - 
                             * this allows us to parse arguments in any order
                             * (because here we are currently only iterating 
                             * through the arguments in a linear fashion) */
                            enableArgProvided = true;
                            break;

                        case "--disable":
                        case "-d":
                            disableArgProvided = true;
                            break;

                        case "--help":
                        case "-h":
                            DisplayArgumentsHelp();
                            Environment.Exit(0);
                            break;
                        
                        // Unrecognised arguments
                        default:

                            /* This condition is required for the same reason
                             * described when we set argsProvided (line 23) */
                            if (i > 0 && !pathArgProvided)
                            {
                                DisplayArgumentsHelp();
                                Environment.Exit(0);
                            }

                            break;
                    }
                }

                // Main arguments logic
                if (pathSuccessfullySet && !enableArgProvided && !disableArgProvided)
                {
                    MessageBox.Show("Valid path was specified, but no enable " +
                                    "or disable command!",
                                    "HELEN ClearType Control Toggler",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                else if (!pathSuccessfullySet && (enableArgProvided || disableArgProvided))
                {
                    MessageBox.Show("No path specified!",
                                    "HELEN ClearType Control Toggler",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                else if (pathSuccessfullySet && enableArgProvided)
                {
                    instance.UpdateVersionInfo();
                    instance.Patch(true);
                    Environment.Exit(0);
                }
                else if (pathSuccessfullySet && disableArgProvided)
                {
                    instance.UpdateVersionInfo();
                    instance.Patch(false);
                    Environment.Exit(0);
                }

            }
            else if (!argsProvided)
            {
                Application.Run(instance.form);
            }
        }


        static void DisplayArgumentsHelp()
        {
            MessageBox.Show(
                "Supported arguments (any order):\n\n" +

                "--help, -h, any unrecognised argument\n" +
                "Displays this help page.\n\n" +

                "--path, -p\nFollowed by a space then a full " +
                "or relative (from directory tool was launched from) " +
                "path to the HELEN executable " +
                "(*.exe) file in quotes.\n" +
                "e.g., --path \"C:\\Applications\\HELEN.exe\"\n" +
                "Required in combination with --enable or --disable " +
                "parameters.\n\n" +

                "--enable, -e\nPatches the HELEN executable so that " +
                "it has control over ClearType (it will attempt " +
                "to turn ClearType on/off as required - default).\n" +
                "Displays message box indicating success/failure.\n\n" +

                "--disable, -d\nPatches the HELEN executable so that " +
                "it has no control over ClearType (it will never" +
                "attempt to turn ClearType on/off).\n" +
                "Displays message box indicating success/failure.\n\n" +

                "Example usage:\n" +
                "HelenClearTypeToggle -p \"C:\\Applications\\" +
                "HELEN.exe\" --enable\n\n" +

                "If no arguments are specified, the GUI will launch.",

                "HELEN ClearType Control Toggler",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
