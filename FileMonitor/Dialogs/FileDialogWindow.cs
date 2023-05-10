using Microsoft.Win32;

namespace FileMonitor.Models
{
    internal static class FileDialogWindow
    {
        /// <summary>
        /// Open a FileDialog browser for the user to select a file.
        /// </summary>
        /// <returns> A string of the full file path </returns>
        public static string GetPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }
    }
}
