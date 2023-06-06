using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System;

namespace MediaConverter.Pages
{
    /// <summary>
    /// Interaction logic for ApplicationPathSelectorPage.xaml
    /// </summary>
    public partial class ApplicationPathSelectorPage : Page
    {
        private string _selectedPath = string.Empty;

        public ApplicationPathSelectorPage()
        {
            InitializeComponent();
        }

        private void ButtonSelectPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Решение (*.sln)|*.sln";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                _selectedPath = dialog.FileName;
                TextBoxSelectedPath.Text = _selectedPath;
            }
        }

        private void ButtonSubmitPath_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
