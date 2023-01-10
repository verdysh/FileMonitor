using System.Windows;

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
                JsonFile jsonConfig = new JsonFile();
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
