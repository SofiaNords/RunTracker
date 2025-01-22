using RunTracker.Model;
using RunTracker.Repository;
using RunTracker.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RunTracker.Dialog
{
    /// <summary>
    /// Interaction logic for EditRunTypeDialog.xaml
    /// </summary>
    public partial class EditRunTypeDialog : Window
    {
        public EditRunTypeDialog(RunType selectedRunType, IRunTypeRepository runTypeRepository)
        {
            InitializeComponent();

            DataContext = new EditRunTypeViewModel(selectedRunType, runTypeRepository, this);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
