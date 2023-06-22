using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace FileMonitor.FileBackups
{
    /// <summary>
    /// Defines a class for copying monitored files to a backup location.
    /// </summary>
    public class Backup
    {
        private string backupFolder;

        /// <summary>
        /// Defines the <see cref="Backup"/> class constructor. A backup folder location is required.
        /// </summary>
        /// <param name="backupFolder"> User-specified location to place the copied files. </param>
        public Backup(string backupFolder)
        {
            this.backupFolder = backupFolder;
        }

        /// <summary>
        /// Copy all files monitored by the program to the designated backup location.
        /// </summary>
        /// <param name="files"> A collection of file paths to copy. </param>
        public void CopyAll(IEnumerable<string> files)
        {
            string backupFolderWithDateTime = $"{backupFolder}\\Backup{DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}\\";
            foreach (string sourceFile in files)
            {
                string destination = GetFullDestinationPath(sourceFile, backupFolderWithDateTime);
                CreateDirectory(destination);
                File.Copy(sourceFile, destination, true);
            }
        }

        /// <summary>
        /// Copy files that have been changed or updated since the last backup.
        /// </summary>
        /// <param name="files"> A collection of file paths to copy. </param>
        public void CopyUpdated(IEnumerable<string> files)
        {
            string backupLocation = $"{backupFolder}\\Backup\\";
            foreach (string sourceFile in files)
            {
                string destination = GetFullDestinationPath(sourceFile, backupLocation);
                CreateDirectory(destination);
                destination = IncrementFileName(destination);
                File.Copy(sourceFile, destination, true);
            }
        }

        // This method replaces the original path root with the backup destination root.
        private string GetFullDestinationPath(string? sourceFile, string backupFolderWithDateTime)
        {
            // Remove the root from the source path and replace it with the backup folder path.
            string oldRoot = Path.GetPathRoot(sourceFile);
            return sourceFile.Replace(oldRoot, backupFolderWithDateTime);
        }

        // Creates the directory path if it does not exist.
        private void CreateDirectory(string path)
        {
            string directory = Path.GetDirectoryName(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            if (!directoryInfo.Exists) directoryInfo.Create();
        }

        // If the file already exists, append numbered text to the file name. Example: "someFile(Copy 3).txt"
        private string IncrementFileName(string fileName)
        {
            int count = 2;
            while(true)
            {
                if (File.Exists(fileName))
                {
                    string extension = Path.GetExtension(fileName);
                    string fileNoExtension = Path.ChangeExtension(fileName, null);
                    if (FileIsNumbered(fileNoExtension, out string matchToStrip))
                    {
                        int charsToRemove = fileNoExtension.Length - matchToStrip.Length;
                        Debug.WriteLine($"chars to remove: {charsToRemove}");
                        fileNoExtension = fileNoExtension.Remove(charsToRemove);
                        fileName = $"{fileNoExtension}(Copy {count}){extension}";
                    }
                    else fileName = $"{fileNoExtension}(Copy {count}){extension}";
                    count++;
                }
                else return fileName;
            }
        }

        // Search for a pattern match on the file name. If a match is found, return true. 
        // Use a ref out parameter to return the regex match.
        private bool FileIsNumbered(string fileName, out string matchToStrip)
        {
            string pattern = @"\(Copy\s(\d+)\)"; // Example match: "(Copy 12)"
            Regex regex = new Regex(pattern);
            Match match = regex.Match(fileName);
            if (match.Success)
            {
                matchToStrip = match.Value;
                Debug.WriteLine($"match.Value: {match.Value}");
                return true;
            }
            else
            {
                matchToStrip = "";
                return false;
            }

            
        }
    }
}
