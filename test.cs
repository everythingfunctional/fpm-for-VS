using EnvDTE80;
using fpm_for_VS.Options;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace fpm_for_VS
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class test
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = PackageIds.testId;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = PackageGuids.guidfpm_for_VSPackageCmdSet;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="test"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private test(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static test Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in test's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new test(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void Execute(object sender, EventArgs e)
        {
            /// (c) 2021 Sourcery, Inc.
            /// This software was developed for the U.S.Nuclear Regulatory Commission(US NRC) under contract # 31310020D0006:
            /// "Technical Assistance in Support of NRC Nuclear Regulatory Research for Materials, Waste, and Reactor Programs"

            try
            {
                await CommandBodyAsync();
            }
            catch (Exception ex)
            {
                // Generic last-chance MessageBox display 
                // to ensure the async exception can't kill Visual Studio.
                // Note that software for end-users (as opposed to internal tools)
                // should usually log these details instead of displaying them directly to the user.
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                VsShellUtilities.ShowMessageBox(
                    this.package,
                    ex.ToString(),
                    "Command failed",
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        private async Task CommandBodyAsync()
        {
            /// (c) 2021 Sourcery, Inc.
            /// This software was developed for the U.S.Nuclear Regulatory Commission(US NRC) under contract # 31310020D0006:
            /// "Technical Assistance in Support of NRC Nuclear Regulatory Research for Materials, Waste, and Reactor Programs"

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            DTE2 dte2 = Package.GetGlobalService(typeof(EnvDTE.DTE)) as DTE2;

            IVsOutputWindow outWindow = (IVsOutputWindow)Package.GetGlobalService(typeof(SVsOutputWindow));
            Guid paneGuid = new Guid();
            outWindow.CreatePane(paneGuid, "fpm Output", Convert.ToInt32(true), Convert.ToInt32(false));
            outWindow.GetPane(ref paneGuid, out IVsOutputWindowPane outputPane);

            string fpmCommand =
                "fpm.exe test"
                    + (string.IsNullOrEmpty(GeneralOptions.Instance.compiler) ? "" : " --compiler " + GeneralOptions.Instance.compiler)
                    + (string.IsNullOrEmpty(GeneralOptions.Instance.profile) ? "" : " --profile " + GeneralOptions.Instance.profile)
                    + (string.IsNullOrEmpty(GeneralOptions.Instance.flags) ? "" : " --flag " + GeneralOptions.Instance.flags)
                    + (string.IsNullOrEmpty(TestOptions.Instance.target) ? "" : " --target " + TestOptions.Instance.target)
                    + (string.IsNullOrEmpty(TestOptions.Instance.extraArgs) ? "" : " -- " + TestOptions.Instance.extraArgs);

            ProcessStartInfo start_info = new ProcessStartInfo
            {
                Arguments = "/k",
                FileName = "cmd.exe",
                WorkingDirectory = dte2.Solution.FullName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            outputPane.OutputString("Starting fpm test\n");
            outputPane.Activate();
            Process proc = Process.Start(start_info);
            proc.OutputDataReceived += (outputSender, args) => outputPane.OutputStringThreadSafe(args.Data + "\n");
            proc.ErrorDataReceived += (outputSender, args) => outputPane.OutputStringThreadSafe(args.Data + "\n");
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            if (!string.IsNullOrEmpty(GeneralOptions.Instance.preExecScript)) await proc.StandardInput.WriteLineAsync(GeneralOptions.Instance.preExecScript).ConfigureAwait(true);
            await proc.StandardInput.WriteLineAsync(fpmCommand).ConfigureAwait(true);
            await proc.StandardInput.WriteLineAsync("exit").ConfigureAwait(true);
            proc.WaitForExit();
        }
    }
}
