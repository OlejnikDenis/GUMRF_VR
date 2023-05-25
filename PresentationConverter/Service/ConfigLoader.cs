using System;
using System.IO;
using System.Windows.Forms;

namespace PresentationConverter
{
    internal static class ConfigLoader
    {
        public static string tempSaveFileName = "settings.txt";

        public static string GetTempFolderPath()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var tempFolderPath = Path.Combine(localAppData, Application.ProductName);

            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            return tempFolderPath;
        }

        public static string GetTempFilePath()
        {
            var tempFolderPath = GetTempFolderPath();
            if (tempFolderPath == null) return null;

            string fullSaveFilePath = Path.Combine(tempFolderPath, tempSaveFileName);
            if (File.Exists(fullSaveFilePath))
            {
                return fullSaveFilePath;
            }
            else
            {
                return null;
            }
        }

        public static string GetTempFileData()
        {
            var filePath = GetTempFilePath();
            var fileData = File.ReadAllText(filePath);

            return fileData;
        }

        public static void SetTempFileData(string data)
        {
            var filePath = GetTempFilePath();
            File.WriteAllText(filePath, data);
        }

        public static bool TempFileExists()
        {
            var filePath = GetTempFilePath();
            return File.Exists(filePath);
        }
    }
}
