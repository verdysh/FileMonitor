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
            textBlockDisplay.ShowAllFiles(this);
        }

        /// <summary>
        /// Update JSON file, and update the Files property in textBlockDisplay in order to trigger
        /// the PropertyChanged event
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();
            if(newFile != "")
            {
                JsonFile.WriteToFile(newFile);
                textBlockDisplay.PropertyChanged += TextBlockDisplay_PropertyChanged;
                textBlockDisplay.Files = JsonFile.GetDeserializedList();
            }
        }

        /// <summary>
        /// Update XAML TextBlocks when the PropertyChanged event is fired
        /// </summary>
        /// <remarks> Pass this MainWindow instance by reference to both method calls </remarks>
        private void TextBlockDisplay_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            textBlockDisplay.ShowAllFiles(this); 
            textBlockDisplay.ShowRecentlyChangedFiles(this);
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
