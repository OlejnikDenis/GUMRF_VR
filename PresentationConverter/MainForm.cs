using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PresentationConverter
{
    public partial class MainForm : Form
    {
        private MessageService _messageService;
        public MainForm()
        {
            InitializeComponent();

            SetupComboBoxValues();

            _messageService = new MessageService(toolStripStatusLabel);

            openFileDialog.Filter = "Презентация PowerPoint (*.pptx)|*.pptx|Презентация PowerPoint 93-2003 (*.ppt)|*.ppt|Все файлы (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
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

            //string filePath = openFileDialog
            string filename = openFileDialog.FileName;

            textBoxPath.Text = filename;
            _messageService.SentMessage($"Загружено: {filename}");
        }
    }
}
