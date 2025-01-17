using Microsoft.Extensions.Configuration;
using RunTracker.Command;
using RunTracker.Model;
using RunTracker.Repository;
using RunTracker.Services;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RunTracker.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private IRunningSessionRepository _runningSessionRepository;

        private readonly IConfiguration? _configuration;

        private DatabaseService _databaseService;

        public ObservableCollection<RunningSession> RunningSessions { get; set; } = new ObservableCollection<RunningSession>();

        private RunningSession _selectedRunningSession;
        public RunningSession SelectedRunningSession
        {
            get { return _selectedRunningSession; }
            set
            {
                if (_selectedRunningSession != value)
                {
                    _selectedRunningSession = value;
                    RaisePropertyChanged();

                    if (_selectedRunningSession != null)
                    {
                        // Uppdatera de andra fälten om vi har ett valt pass
                        Date = _selectedRunningSession.Date;
                        Distance = _selectedRunningSession.Distance;
                        Time = _selectedRunningSession.Time;
                        RunType = _selectedRunningSession.RunType;

                        RaisePropertyChanged(nameof(Date));
                        RaisePropertyChanged(nameof(Distance));
                        RaisePropertyChanged(nameof(Time));
                        RaisePropertyChanged(nameof(RunType));
                    }
                }
            }
        }

        public DateTime? Date { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public string RunType { get; set; }

        public DelegateCommand AddRunningSessionCommand { get; }
        public DelegateCommand UpdateRunningSessionCommand { get; }
        public DelegateCommand DeleteRunningSessionCommand { get; }

        public MainWindowViewModel(IConfiguration? configuration)
        {
            _configuration = configuration;

            AddRunningSessionCommand = new DelegateCommand(async _ => await AddRunningSessionAsync());
            //UpdateRunningSessionCommand = new DelegateCommand(async _ => await UpdateRunningSessionAsync());
            DeleteRunningSessionCommand = new DelegateCommand(async _ => await DeleteRunningSessionAsync());

            ConnectToDatabase();
            LoadRunningSessions();
        }

        private void ConnectToDatabase()
        {
            _databaseService = new DatabaseService(_configuration);
            _runningSessionRepository = new RunningSessionRepository(_databaseService.Database);
        }

        private async Task LoadRunningSessions()
        {
            var sessions = await _runningSessionRepository.GetAllAsync();
            RunningSessions.Clear();
            foreach (var session in sessions)
            {
                RunningSessions.Add(session);
            }

        }
        private async Task AddRunningSessionAsync()
        {
            var newSession = new RunningSession
            {
                Date = (DateTime)Date,
                Distance = Distance,
                Time = Time,
                RunType = RunType
            };
            await _runningSessionRepository.AddAsync(newSession);
            await LoadRunningSessions(); // Reload the list after adding
        }

        private async Task DeleteRunningSessionAsync()
        {
            if (SelectedRunningSession != null)
            {
                await _runningSessionRepository.DeleteAsync(SelectedRunningSession.Id);
                RunningSessions.Remove(SelectedRunningSession);
            }
        }
    }
}
