﻿using Kool.VsDiff.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Kool.VsDiff.Pages
{
    internal sealed partial class DiffToolOptionsPage : UserControl
    {
        private readonly VsDiffOptions _options;

        public DiffToolOptionsPage(VsDiffOptions options)
        {
            InitializeComponent();
            DataContext = _options = options;
        }

        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(_options.CustomDiffToolPath))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(_options.CustomDiffToolPath);
            }
            if (dialog.ShowDialog() == true)
            {
                CustomDiffToolPath.Text = dialog.FileName;
            }
        }

        private void OnTestButtonClicked(object sender, RoutedEventArgs e)
        {
            var file1 = TempFileHelper.CreateTempFile("File1", ".tmp", "$FILE1");
            var file2 = TempFileHelper.CreateTempFile("File2", ".tmp", "$FILE2");

            try
            {
                DiffToolFactory.CreateDiffTool().Diff(file1, file2);
            }
            catch (Exception ex)
            {
                //TODO:localization
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}