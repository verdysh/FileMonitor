using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VinPCGS
{
    /// <summary>
    /// Defines all methods for reading from and writing to the storedPath.json file.
    /// </summary>
    internal class JsonFile
    {
        private static readonly string programFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\backups";
        private static readonly string storedPaths = $"{programFolder}\\storedPaths.json";

        /// <summary>
        /// Instantiate an object of type JsonSerializerOptions. Set WriteIndented property to true 
        /// in order to format any serialized / deserialized JSON data with white spaces.
        /// </summary>
        /// <returns> JsonSerializerOptions object to format a JSON file with white spaces. </returns>
        private JsonSerializerOptions GetWhiteSpaceFormatting()
        {
            JsonSerializerOptions jsonSettings = new JsonSerializerOptions();
            jsonSettings.WriteIndented = true;
            return jsonSettings;
        }

        /// <summary>
        /// Create the storedPath.json file with an empty string List.
        /// </summary>
        private void CreateNewFile()
        {
            List<string> emptyFile = new List<string>();
            string jsonData = JsonSerializer.Serialize(emptyFile, this.GetWhiteSpaceFormatting());
            File.WriteAllText(storedPaths, jsonData);
        }

        /// <summary>
        /// Read all JSON text into a string. Deserialize the string, then return the List
        /// </summary>
        /// <returns> A string list of all stored file paths </returns>
        public List<string> GetDeserializedList()
        {
            string jsonFileData = File.ReadAllText(storedPaths);
            List<string> jsonList = JsonSerializer.Deserialize<List<string>>(jsonFileData);
            return jsonList;
        }

        /// <summary>
        ///  Deserialize the JSON file as a string List, add a new file path, re-serialize the list, then save over the file. 
        /// </summary>
        /// <remarks> This method creates the program folder and the JSON file if they do not exist. </remarks>
        public void WriteToFile(string newPath)
        {
            if (!File.Exists(storedPaths) || !Directory.Exists(programFolder))
            {
                Directory.CreateDirectory(programFolder);
                CreateNewFile();
            }

            List<string> jsonList = GetDeserializedList();   
            jsonList.Add(newPath);

            string serializedList = JsonSerializer.Serialize(jsonList, this.GetWhiteSpaceFormatting());
            File.WriteAllText(storedPaths, serializedList);
        }
    }
}
