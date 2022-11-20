using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace VinPCGS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialogWindow window = new FileDialogWindow();
            string newFile = window.GetPath();
            if(newFile != "")
            {
                JsonFileConfiguration jsonConfig = new JsonFileConfiguration();
                jsonConfig.WriteToFile(newFile);
            }
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
