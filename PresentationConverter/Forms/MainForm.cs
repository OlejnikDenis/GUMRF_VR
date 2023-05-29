using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PresentationConverter
{
    public partial class MainForm : Form
    {
        private MessageService _messageService;
        private PathSelectorForm _pathSelectorForm;

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
            _messageService.SentMessage($"Открыто: {filename}.");
            buttonSubmit.Enabled = true;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            var subfolder = comboBoxLocation.SelectedValue.ToString();
            var _unityAppPath = ConfigLoader.GetTempFileData();

            Converter.ConvertToImage(textBoxPath.Text, $"{_unityAppPath}\\Assets\\Media\\{subfolder}");
            
            _messageService.SentMessage("Готово. Все файлы успешно переконвертированы.");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!ConfigLoader.TempFileExists() || !ConfigLoader.TempFileCorrect())
            {
                _messageService.SentMessage("Файл конфигурации не найден.");

                _pathSelectorForm = new PathSelectorForm();
                _pathSelectorForm.ShowDialog();
            }
            _messageService.SentMessage("Готов к работе.");

        }
    }
}
