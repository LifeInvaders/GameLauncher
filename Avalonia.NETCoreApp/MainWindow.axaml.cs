using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;


namespace Avalonia.NETCoreApp
{
    public class MainWindow : Window
    {
        private Button downloadButton;
        public string Student;
        private Client client;
        
        public string FirstName { get; }
        public string LastName { get; }
        
        
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();

#endif
        }
        public void DownloadClick(object sender, RoutedEventArgs e)
        {
            client.Update();
            client.Play();
            Close();
        }

        private void InitializeComponent()
        {
            Student = "";
            client = new Client();
            AvaloniaXamlLoader.Load(this);
            downloadButton = this.FindControl<Button>("Download");
            // downloadButton.Name = "toto";
        }
    }
}