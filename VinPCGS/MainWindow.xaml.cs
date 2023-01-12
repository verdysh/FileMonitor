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
                textBlockDisplay.PropertyChanged += TextBlockDisplay_PropertyChanged;
            }
        }

        /// <summary>
        /// Update XAML TextBlocks when the PropertyChanged event is fired
        /// </summary>
        private void TextBlockDisplay_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            textBlockDisplay.ShowAllFiles();
            textBlockDisplay.ShowRecentlyChangedFiles();
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
