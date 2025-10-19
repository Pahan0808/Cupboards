using System.IO;
using UnityEngine;

namespace Cupboards
{
    public class FileService
    {
        public string LoadFile(string fileName)
        {
            var configPath = GetPath(fileName);
            if (File.Exists(configPath))
            {
                return File.ReadAllText(configPath);
            }
        
            var textAsset = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(configPath));
            return textAsset?.text;
        }

        private static string GetPath(string fileName)
        {
            return $"{Application.dataPath}/_Cupboards/Resources/{fileName}";
        }
    }
}
