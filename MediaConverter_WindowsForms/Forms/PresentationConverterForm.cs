using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PresentationConverter
{
    public partial class PresentationConverterForm : Form
    {
        private MessageService _messageService;
        private ConverterSelectorForm _pathSelectorForm;
        private string _unityAppPath;
        private string _unityMediaPath;

        public PresentationConverterForm()
        {
            InitializeComponent();

            SetupComboBoxValues();

            _unityAppPath = ConfigLoader.GetTempFileData();
            _unityMediaPath = Path.Combine(_unityAppPath, "Assets", "Media");

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
                new ComboBoxItem { DisplayValue = "Другое", HiddenValue = "Another"},
                new ComboBoxItem { DisplayValue = "Свой вариант", HiddenValue = ""}
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

            Converter.ConvertToImage(textBoxPath.Text, Path.Combine(_unityMediaPath, subfolder));
            
            _messageService.SentMessage("Готово. Все слайды успешно переконвертированы.");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!ConfigLoader.TempFileExists() || !ConfigLoader.TempFileCorrect())
            {
                _messageService.SentMessage("Файл конфигурации не найден.");

                _pathSelectorForm = new ConverterSelectorForm();
                _pathSelectorForm.ShowDialog();
            }
            _messageService.SentMessage("Готов к работе.");
        }

        private void toolStripViewFolder_Click(object sender, EventArgs e)
        {
            var explorerProcessName = "explorer.exe";
            try
            {
                Process.Start(explorerProcessName, _unityMediaPath);
                _messageService.SentMessage("Открыта папка \"Media\".");
                
            }
            catch (Exception ex)
            {
                var fullErrorLog = $"Системе не удалось найти указанный путь: {_unityMediaPath}.\n\n" +
                    $"Message: {ex.Message}.\n\n" +
                    $"StackTrace:\n{ex.StackTrace}.";

                MessageBox.Show(fullErrorLog, $"Процесс \"{explorerProcessName}\" вернул ошибку!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxLocation_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
