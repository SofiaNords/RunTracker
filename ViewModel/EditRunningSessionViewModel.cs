using RunTracker.Command;
using RunTracker.Model;
using RunTracker.Repository;
using System.Collections.ObjectModel;
using System.Windows;

namespace RunTracker.ViewModel
{
    public class EditRunningSessionViewModel : ViewModelBase
    {
        private readonly IRunningSessionRepository _runningSessionRepository;

        private readonly Window _dialog;

        private readonly Func<Task> _onUpdate;

        private RunningSession _selectedRunningSession;

        public RunningSession SelectedRunningSession
        {
            get => _selectedRunningSession;
            set 
            { 
                _selectedRunningSession = value;
                RunType = _selectedRunningSession.RunType;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<RunType> _runTypes;
        public ObservableCollection<RunType> RunTypes
        {
            get => _runTypes;
            set
            {
                _runTypes = value;
                RaisePropertyChanged();
            }
        }

        private RunType _runType;
        public RunType RunType
        {
            get => _runType;
            set
            {
                _runType = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateRunningSessionCommand { get; }

        public EditRunningSessionViewModel(RunningSession selectedSession, ObservableCollection<RunType> runTypes, IRunningSessionRepository runningSessionRepository, Window dialog, Func<Task> onUpdate)
        {
            SelectedRunningSession = selectedSession;
            RunTypes = runTypes;
            RunType = selectedSession.RunType;
            _runningSessionRepository = runningSessionRepository;
            _dialog = dialog;
            _onUpdate = onUpdate;

            UpdateRunningSessionCommand = new DelegateCommand(async _ => await UpdateRunningSessionAsync());
        }

        public async Task UpdateRunningSessionAsync()
        {
            SelectedRunningSession.Date = DateTime.SpecifyKind((DateTime)SelectedRunningSession.Date, DateTimeKind.Utc);
            SelectedRunningSession.RunType = RunType;
            try
            {
                await _runningSessionRepository.UpdateAsync(SelectedRunningSession);
                await _onUpdate();
                _dialog.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the running session: {ex.Message}");
            }


        }

    }
}
