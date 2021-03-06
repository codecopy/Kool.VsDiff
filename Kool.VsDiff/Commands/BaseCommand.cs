﻿using Kool.VsDiff.Models;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using static Kool.VsDiff.Models.VS;

namespace Kool.VsDiff.Commands
{
    internal abstract class BaseCommand : OleMenuCommand
    {
        protected BaseCommand(VsDiffPackage package, string cmdSet, int cmdId)
            : base(OnBaseCommandEventHandler, null, OnBaseBeforeQueryStatus, new CommandID(Guid.Parse(cmdSet), cmdId))
        {
            Package = package;
        }

        protected VsDiffPackage Package { get; }

        private static void OnBaseBeforeQueryStatus(object sender, EventArgs e)
        {
            var command = sender as BaseCommand;
            var commandName = command.GetType().Name;

            OutputWindow.Debug($"Command {commandName} OnBeforeQueryStatus.");

            try
            {
                command.OnBeforeQueryStatus();
            }
            catch (Exception ex)
            {
                OutputWindow.Error($"Failed to enable command {commandName}.", ex);
            }
        }

        private static void OnBaseCommandEventHandler(object sender, EventArgs e)
        {
            var command = sender as BaseCommand;
            var commandName = command.GetType().Name;

            OutputWindow.Info($"Execute command {commandName}.");

            try
            {
                command.OnExecute();
            }
            catch (Exception ex)
            {
                MessageBox.Error(VSPackage.ErrorMessageTitle, ex.Message);
                OutputWindow.Error($"Failed to execute command {commandName}.", ex);
            }
        }

        protected virtual void OnBeforeQueryStatus()
        {
        }

        protected abstract void OnExecute();

        public void Turn(bool featureOn)
        {
            if (!featureOn)
            {
                Package.CommandService.RemoveCommand(this);
                return;
            }

            if (featureOn && Package.CommandService.FindCommand(CommandID) == null)
            {
                Package.CommandService.AddCommand(this);
            }
        }
    }
}