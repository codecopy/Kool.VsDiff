﻿using EnvDTE80;
using Kool.VsDiff.Commands;
using Kool.VsDiff.Models;
using Kool.VsDiff.Pages;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Kool.VsDiff
{
    [Guid(Ids.PACKAGE)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(VsDiffOptions), Vsix.PRODUCT, Vsix.PACKAGE, 0, 0, true)]
    [ProvideBindingPath]
    public sealed class VsDiffPackage : Package
    {
        public DTE2 IDE { get; private set; }

        public OleMenuCommandService CommandService { get; private set; }

        internal VsDiffOptions Options { get; private set; }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            IDE = GetService(typeof(EnvDTE.DTE)) as DTE2;
            CommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            Options = GetDialogPage(typeof(VsDiffOptions)) as VsDiffOptions;

            VS.Initialize(this);
            DiffToolFactory.Initialize(this);

            DiffSelectedFilesCommand.Initialize(this);
            DiffClipboardWithCodeCommand.Initialize(this);
            DiffClipboardWithFileCommand.Initialize(this);

            VS.OutputWindow.Info("Package is sited and initialized.");
        }
    }
}