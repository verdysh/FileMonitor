using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VinPCGS
{
    internal class FileDialogWindow
    {
        /// <summary>
        /// Open a FileDialog browser for the user to select a file.
        /// </summary>
        /// <returns> A string of the full file path </returns>
        public string GetPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                return openFileDialog.FileName;
            }
            else
            {
                return "";
            }
        }
    }
}
