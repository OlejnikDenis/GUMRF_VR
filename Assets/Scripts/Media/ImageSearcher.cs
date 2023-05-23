using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Media
{
    /// <summary>
    /// A class used for searching image paths in a given folder and its subfolders with a given file extension.
    /// </summary>
    /// <remarks>
    /// The class searches for image paths in the specified folder and its subfolders with a given file extension.
    /// It then stores them in the list of strings for later use.
    /// </remarks>

    public class ImageSearcher : MonoBehaviour
    {
        [SerializeField] private string _subfolder = "";
        private string mediaPath { get; } = Application.dataPath + "/Media/";
        public List<string> imagePaths { get; private set; } = new();

        private const string ImageExtension = ".jpg";

        private void Start()
        {
            imagePaths = GetImagePaths(mediaPath, _subfolder, ImageExtension);
        }
    
        /// <summary>
        /// Searches for image paths in a specified folder and its subfolders by given file extension.
        /// </summary>
        /// <param name="path">The root folder path.</param>
        /// <param name="location">The subfolder name.</param>
        /// <param name="extension">File extension to search for, e.g. ".png" or ".jpg".</param>
        /// <returns>List of found file paths or null if the folder doesn't exist.</returns>
        private static List<string> GetImagePaths(string path, string location, string extension)
        {
            var foundedPaths = new List<string>();
            var searchPath = Path.Combine(path, location);
        
            // Check if the directory exists
            if (!Directory.Exists(searchPath))
            {
                Debug.LogError($"The folder path does not exist ({searchPath})");
                return null;
            }
        
            // Search for files with given extensions and add their paths to the list
            var filePaths = Directory.GetFiles(searchPath, $"*{extension}", SearchOption.TopDirectoryOnly);
            foundedPaths.AddRange(filePaths.ToList());

            return foundedPaths;
        }
    
    }
}
