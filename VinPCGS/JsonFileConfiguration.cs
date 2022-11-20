using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VinPCGS
{
    /// <summary>
    /// Defines all methods for reading from and writing to the inventory.json file. This file is stored in the 
    /// user's home directory.
    /// </summary>
    internal class JsonFileConfiguration
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
        /// Create the inventory.json file. 
        /// </summary>
        private void CreateNewFile()
        {
            List<string> emptyFile = new List<string>();
            string jsonData = JsonSerializer.Serialize(emptyFile, this.GetWhiteSpaceFormatting());
            File.WriteAllText(storedPaths, jsonData);
        }

        /// <summary>
        /// Read all JSON text into a string. Deserialize the string, then return the List as specified by the user
        /// </summary>
        /// <returns> List of Inventory objects from inventory.json </returns>
        public List<string> GetDeserializedList()
        {
            string jsonFileData = File.ReadAllText(storedPaths);
            List<string> jsonList = JsonSerializer.Deserialize<List<string>>(jsonFileData);
            return jsonList;
        }

        /// <summary>
        /// Write a new Inventory object to inventory.json. First call GetJsonAsString() to retrieve the JSON file data. 
        /// Deserialize the string into a List of Inventory objects, append the new Inventory object to the list. Then, 
        /// re-serialize the List and save over the inventory.json file. 
        /// </summary>
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
