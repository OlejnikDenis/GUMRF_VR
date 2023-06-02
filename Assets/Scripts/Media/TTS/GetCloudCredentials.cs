using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Media.TTS
{
    public class GetCloudCredentials : MonoBehaviour
    {
        public static string GetIamToken()
        {
            const string command = "yc";
            const string args = "iam create-token";
            var iamToken = RunCommand(command, args);
            
            return iamToken.Trim();
        }

        public static string GetFolderId(string folderName)
        {
            const string command = "yc";
            var args = $"resource-manager folder get {folderName}";
            var output = RunCommand(command, args);
            var folderId = output.Split("id: ")[1][..20];
            
            return folderId;
        }

        private static string RunCommand(string command, string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = new Process
            {
                StartInfo = startInfo,
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}
