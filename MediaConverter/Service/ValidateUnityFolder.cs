using System.IO;

namespace PresentationConverter.Service
{
    internal static class ValidateUnityFolder
    {
        ///<summary>
        ///This method checks if the given path is a valid Unity folder by verifying if it contains the "Assets" and "ProjectSettings" folders 
        ///and if there are any ".meta" files inside it's "Assets" folder.
        ///</summary>
        ///<param name="path">The path to the folder that needs to be validated.</param>
        ///<returns>A boolean value indicating whether the given path is a valid Unity folder or not.</returns>
        public static bool isUnityFolder(string path)
        {
            //Combining the given path with the required subfolders to verify if those folders exist.
            var assetsFolder = Path.Combine(path, "Assets");
            var projectSettingsFolder = Path.Combine(path, "ProjectSettings");

            //Checking if the required folders exist in the given path.
            if (Directory.Exists(assetsFolder) && Directory.Exists(projectSettingsFolder))
            {
                //Getting all files with ".meta" extension inside the "Assets" folder and it's subfolders.
                string[] metaFiles = Directory.GetFiles(assetsFolder, "*.meta", SearchOption.AllDirectories);

                //Checking if there's any ".meta" file present.
                if (metaFiles.Length > 0) 
                    return true;

                //If there's no ".meta" file present, then we can't consider this a valid Unity folder.
                return false; 
            }
            
            //If the required folders don't exist, then we can't consider this a valid Unity folder.
            return false; 
        }
    }
}
