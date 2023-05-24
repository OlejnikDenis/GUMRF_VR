using Microsoft.Office.Interop.PowerPoint;
using System.IO;
using Microsoft.Office.Core;

namespace PresentationConverter
{
    public class Converter
    {
        public static void ConvertToImage(string inputFilePath, string outputFolderPath, string imageExtension = "jpg")
        {
            Application app = new Application();

            Presentation presentation = app.Presentations.Open(inputFilePath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

            int slideWidth = (int)presentation.PageSetup.SlideWidth;
            int slideHeight = (int)presentation.PageSetup.SlideHeight;

            for (int i = 1; i <= presentation.Slides.Count; i++)
            {
                Slide slide = presentation.Slides[i];
                string outputPath = Path.Combine(outputFolderPath, $"Slide_{i:D3}.{imageExtension}");

                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }

                slide.Export(outputPath, imageExtension, slideWidth, slideHeight);
            }

            presentation.Close();
            app.Quit();
        }
    }
}
