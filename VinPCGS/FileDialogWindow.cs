using Microsoft.Win32;

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
