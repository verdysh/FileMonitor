﻿using System;
using System.Collections.Generic;
using System.IO;

namespace FileMonitor.FileBackups
{
    /// <summary>
    /// Defines a class to handle backing up files that are monitored by the program
    /// </summary>
    internal class FullBackup
    {
        private string backupFolder;

        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="backupFolder"> User-specified location to place the copied files </param>
        public FullBackup(string backupFolder)
        {
            this.backupFolder = backupFolder;
        }

        /// <summary>
        /// A method to execute the backup
        /// </summary>
        /// <param name="files"> A collection of file paths to copy </param>
        public void Run(IEnumerable<string> files)
        {
            string backupFolderWithDateTime = $"{backupFolder}\\Backup{DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}\\";
            foreach (string sourceFile in files)
            {
                string destination = GetFullDestinationPath(sourceFile, backupFolderWithDateTime);
                CreateDirectory(destination);
                File.Copy(sourceFile, destination, true);
            }
        }


        private string GetFullDestinationPath(string? sourceFile, string backupFolderWithDateTime)
        {
            // Remove the root from the source path and replace it with the backup folder path
            string oldRoot = Path.GetPathRoot(sourceFile);
            return sourceFile.Replace(oldRoot, backupFolderWithDateTime);
        }

        private void CreateDirectory(string path)
        {
            string directory = Path.GetDirectoryName(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            if (!directoryInfo.Exists) directoryInfo.Create();
        }
    }
}
