using Microsoft.Extensions.Configuration;
using System.Windows;
using RunTracker.ViewModel;
using System.IO;

namespace RunTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IConfiguration _configuration;
        //public IConfiguration Configuration
        //{
        //    get { return _configuration; }
        //    private set; 
        //}
        public MainWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<MainWindow>();

            _configuration = builder.Build();

            var viewModel = new MainWindowViewModel(_configuration);

            this.DataContext = viewModel;

        }

        
    }
}