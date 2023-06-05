using PresentationConverter.Service;
using System;
using System.IO;
using System.Windows.Forms;

namespace PresentationConverter
{
    public partial class ConverterSelectorForm : Form
    {
        public string SelectedAppFilePath { get; private set; }
        public string SelectedAppFolderPath { get; private set; }

        public ConverterSelectorForm()
        {
            InitializeComponent();
        }

        private void buttonSelectApp_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;

            SelectedAppFilePath = openFileDialog.FileName;
            SelectedAppFolderPath = Path.GetDirectoryName(SelectedAppFilePath);

            if (ValidateUnityFolder.isUnityFolder(SelectedAppFolderPath))
            {
                ConfigLoader.SetTempFileData(SelectedAppFolderPath);
                this.Close();
            }
        }
    }
}
