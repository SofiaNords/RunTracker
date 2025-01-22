using RunTracker.Model;
using RunTracker.Repository;
using RunTracker.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RunTracker.Dialog
{
    /// <summary>
    /// Interaction logic for EditRunningSessionDialog.xaml
    /// </summary>
    public partial class EditRunningSessionDialog : Window
    {
        public EditRunningSessionDialog(RunningSession selectedSession, ObservableCollection<RunType> runTypes, IRunningSessionRepository runningSessionRepository, Func<Task> onUpdate)
        {
            InitializeComponent();

            DataContext = new EditRunningSessionViewModel(selectedSession, runTypes, runningSessionRepository, this, onUpdate);
        }
    }
}
