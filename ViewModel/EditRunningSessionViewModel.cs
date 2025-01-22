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

        private RunningSession _selectedRunningSession;

        public RunningSession SelectedRunningSession
        {
            get => _selectedRunningSession;
            set 
            { 
                _selectedRunningSession = value;
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

        public EditRunningSessionViewModel(RunningSession selectedSession, ObservableCollection<RunType> runTypes, IRunningSessionRepository runningSessionRepository, Window dialog)
        {
            SelectedRunningSession = selectedSession;
            RunTypes = runTypes;
            RunType = selectedSession.RunType;
            _runningSessionRepository = runningSessionRepository;
            _dialog = dialog;

            UpdateRunningSessionCommand = new DelegateCommand(async _ => await UpdateRunningSessionAsync());
        }

        public async Task UpdateRunningSessionAsync()
        {

            SelectedRunningSession.RunType = RunType;
            try
            {
                await _runningSessionRepository.UpdateAsync(SelectedRunningSession);
                MessageBox.Show("Löpsessionen har uppdaterats.");
                _dialog.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod vid uppdatering av löppasset: {ex.Message}");
            }


        }

    }
}
