using System.Windows;

namespace VinPCGS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This object fires an event when the list of files have changed
        /// </summary>
        FileTextBlockDisplay textBlockDisplay = new FileTextBlockDisplay();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();
            if(newFile != "")
            {
                JsonFile.WriteToFile(newFile);
                textBlockDisplay.Files = JsonFile.GetDeserializedList();
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
