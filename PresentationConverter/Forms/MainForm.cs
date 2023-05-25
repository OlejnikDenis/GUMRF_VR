using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PresentationConverter
{
    public partial class MainForm : Form
    {
        private MessageService _messageService;
        private PathSelectorForm _pathSelectorForm;

        private string _unityAppPath = "D:\\Projects\\Unity\\GUMRF_VR";

        public MainForm()
        {
            InitializeComponent();

            SetupComboBoxValues();

            _messageService = new MessageService(toolStripStatusLabel);
            _messageService.SentMessage("Готов к работе");
        }

        /// <summary>
        /// The method creates variants for the ComboBox, and adjusts the display items
        /// </summary>
        private void SetupComboBoxValues()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>()
            {
                new ComboBoxItem { DisplayValue = "Актовый зал", HiddenValue = "AssemblyHall" },
                new ComboBoxItem { DisplayValue = "Другое", HiddenValue = "Another"}
            };

            comboBoxLocation.DisplayMember = "DisplayValue";
            comboBoxLocation.ValueMember = "HiddenValue";

            comboBoxLocation.DataSource = items;
            comboBoxLocation.Sorted = true;
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;

            string filename = openFileDialog.FileName;

            textBoxPath.Text = filename;
            _messageService.SentMessage($"Открыто: {filename}");
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            var subfolder = "test";
            Converter.ConvertToImage(textBoxPath.Text, $"{_unityAppPath}\\Assets\\Media\\{subfolder}");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!ConfigLoader.TempFileExists())
            {
                _pathSelectorForm = new PathSelectorForm();
                _pathSelectorForm.ShowDialog();
            }
            
        }
    }
}
