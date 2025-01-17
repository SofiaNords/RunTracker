using Microsoft.Extensions.Configuration;
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

                    Date = _selectedRunningSession?.Date;
                    Distance = (double)(_selectedRunningSession?.Distance);
                    Time = (TimeSpan)(_selectedRunningSession?.Time);
                    RunType = _selectedRunningSession?.RunType;

                    RaisePropertyChanged(nameof(Date));
                    RaisePropertyChanged(nameof(Distance));
                    RaisePropertyChanged(nameof(Time));
                    RaisePropertyChanged(nameof(RunType));
                }
            }
        }

        public DateTime? Date { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public string RunType { get; set; }

        public MainWindowViewModel(IConfiguration? configuration)
        {
            _configuration = configuration;
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
    }
}
