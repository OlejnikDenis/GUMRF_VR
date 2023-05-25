using PresentationConverter.Service;
using System;
using System.IO;
using System.Windows.Forms;

namespace PresentationConverter
{
    ///<summary>
    ///Contains methods for managing configuration settings.
    ///</summary>
    internal static class ConfigLoader
    {
        ///<summary>
        ///Specifies the name of the temporary save file.
        ///</summary>
        public static string tempSaveFileName = "settings.txt";

        ///<summary>
        ///Returns the path to the temporary folder to store configuration settings.
        ///</summary>
        ///<returns>The path to the temporary folder.</returns>
        public static string GetTempFolderPath()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var tempFolderPath = Path.Combine(localAppData, $"_{Application.ProductName}");

            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            return tempFolderPath;
        }

        ///<summary>
        ///Returns the path to the temporary file used to store configuration data.
        ///</summary>
        ///<returns>The path to the file.</returns>
        public static string GetTempFilePath()
        {
            var tempFolderPath = GetTempFolderPath();
            if (tempFolderPath == null) return null;

            string fullSaveFilePath = Path.Combine(tempFolderPath, tempSaveFileName);
            if (!File.Exists(fullSaveFilePath))
            {
                File.Create(fullSaveFilePath);
            }

            return fullSaveFilePath;
        }

        ///<summary>
        ///Returns the contents of the temporary file used to store configuration data.
        ///</summary>
        ///<returns>The data stored in the file.</returns>
        public static string GetTempFileData()
        {
            var filePath = GetTempFilePath();
            var fileData = File.ReadAllText(filePath);

            return fileData;
        }

        ///<summary>
        ///Writes the provided data to the temporary file used to store configuration data.
        ///</summary>
        ///<param name="data">The data to be saved to the file.</param>
        public static void SetTempFileData(string data)
        {
            var filePath = GetTempFilePath();
            File.WriteAllText(filePath, data);
        }

        ///<summary>
        ///Checks whether the temporary file used to store configuration data exists.
        ///</summary>
        ///<returns>True if the file exists; otherwise, false.</returns>
        public static bool TempFileExists()
        {
            var filePath = GetTempFilePath();
            return File.Exists(filePath);
        }

        ///<summary>
        ///Checks whether the contents of the temporary file used to store configuration data are valid.
        ///</summary>
        ///<returns>True if the contents of the file are valid; otherwise, false.</returns>
        public static bool TempFileCorrect()
        {
            var filePath = GetTempFilePath();
            var fileContent = File.ReadAllText(filePath);

            if (ValidateUnityFolder.isUnityFolder(fileContent)) return true;
            return false
        }
    }
}
