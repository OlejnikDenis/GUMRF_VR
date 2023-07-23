using System;
using System.IO;
using UnityEngine;

public class PresentationFoldersManager : MonoBehaviour
{
    public const string ApplicationName = "GUMRF_VR";

    /// <summary>
    /// The method returns the path to "/AppData/Local/GUMRF_VR". If it does not exist, it will be created. 
    /// </summary>
    /// <returns>Path /AppData/Local/GUMRF_VR</returns>
    public string GetLocalAppDataPath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var applicationPath = Path.Combine(localAppData, ApplicationName);

        return Directory.Exists(applicationPath) ? applicationPath : CreateLocalAppDataFolder(applicationPath);
    }

    //TODO: FIX THAT CRASH!
    
    /// <summary>
    /// Creates a specified folder at the provided relative path within the Local Application Data directory.
    /// </summary>
    /// <param name="relativePath">The relative path of the folder to be created.</param>
    /// <returns>The full path of the created folder.</returns>
    public string CreateSpecifiedFolder(string relativePath)
    {
        try
        {
            var newFolderPath = Path.Combine(GetLocalAppDataPath(), relativePath);
            Directory.CreateDirectory(newFolderPath);
            
            Debug.Log($"Created: \"{newFolderPath}\"");
            
            return newFolderPath;
        }
        catch (Exception ex)
        {
            var message = $"Error when creating a folder: {ex.Message}";
            Debug.LogError(message);
            Console.WriteLine(message);
            
            throw;
        }
    }
        
    /// <summary>
    /// Creates a folder at the specified path within the LocalApplicationData directory.
    /// </summary>
    /// <param name="applicationPath">The full path where the folder should be created.</param>
    /// <returns>The full path of the created folder if successful, otherwise throws an exception.</returns>
    private static string CreateLocalAppDataFolder(string applicationPath)
    {
        try
        {
            Directory.CreateDirectory(applicationPath);
    
            return applicationPath;
        }
        catch (Exception ex)
        {
            var message = $"Error when creating a folder: {ex.Message}";
            Debug.LogError(message);
            Console.WriteLine(message);
            
            throw;
        }
    }

}
