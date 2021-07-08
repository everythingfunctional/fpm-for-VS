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
using Task = System.Threading.Tasks.Task;

namespace fpm_for_VS
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class build
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = PackageIds.buildId;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = PackageGuids.guidfpm_for_VSPackageCmdSet;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="build"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private build(AsyncPackage package, OleMenuCommandService commandService)
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
        public static build Instance
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
            // Switch to the main thread - the call to AddCommand in build's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new build(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            /// (c) 2021 Sourcery, Inc.
            /// This software was developed for the U.S.Nuclear Regulatory Commission(US NRC) under contract # 31310020D0006:
            /// "Technical Assistance in Support of NRC Nuclear Regulatory Research for Materials, Waste, and Reactor Programs"
            
                        ThreadHelper.ThrowIfNotOnUIThread();

            DTE2 dte2 = Package.GetGlobalService(typeof(EnvDTE.DTE)) as DTE2;

            ProcessStartInfo start_info = new ProcessStartInfo
            {
                Arguments =
                    "/k"
                    + (GeneralOptions.Instance.preExecScript == "" ? "" : " " + GeneralOptions.Instance.preExecScript + " & ")
                    + " fpm.exe build"
                    + (GeneralOptions.Instance.compiler == "" ? "" : " --compiler " + GeneralOptions.Instance.compiler)
                    + (GeneralOptions.Instance.profile == "" ? "" : " --profile " + GeneralOptions.Instance.profile)
                    + (GeneralOptions.Instance.flags == "" ? "" : " --flag " + GeneralOptions.Instance.flags),
                FileName = "cmd.exe",
                WorkingDirectory = dte2.Solution.FullName
            };
            Process proc = Process.Start(start_info);
            proc.WaitForExit();
        }
    }
}
