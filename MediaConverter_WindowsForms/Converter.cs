using Microsoft.Office.Interop.PowerPoint;
using System.IO;
using Microsoft.Office.Core;
using System.Windows.Forms;
using PPTApplication = Microsoft.Office.Interop.PowerPoint.Application;
using System.Runtime.CompilerServices;

namespace PresentationConverter
{
    public class Converter
    {
        private static Presentation _presentation;
        private static PPTApplication _app;

        public static void ConvertToImage(string inputFilePath, string outputFolderPath, string imageExtension = "jpg")
        {
            try
            {
                _app = new PPTApplication();

                _presentation = _app.Presentations.Open(inputFilePath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

                int slideWidth = (int)_presentation.PageSetup.SlideWidth;
                int slideHeight = (int)_presentation.PageSetup.SlideHeight;

                for (int i = 1; i <= _presentation.Slides.Count; i++)
                {
                    Slide slide = _presentation.Slides[i];
                    string outputPath = Path.Combine(outputFolderPath, $"Slide_{i:D3}.{imageExtension}");

                    if (!Directory.Exists(outputFolderPath))
                    {
                        Directory.CreateDirectory(outputFolderPath);
                    }

                    if (File.Exists(outputPath))
                    {
                        File.Delete(outputPath);
                    }
                    slide.Export(outputPath, imageExtension, slideWidth, slideHeight);

                }

                _presentation.Close();
                _app.Quit();
            }
            catch (System.Exception e)
            {

                var dialog = MessageBox.Show(e.Message, $"Ошибка", MessageBoxButtons.RetryCancel,MessageBoxIcon.Error);
                if (dialog == DialogResult.Retry)
                {
                    ConvertToImage(inputFilePath, outputFolderPath);
                }
            }
        }
    }
}
