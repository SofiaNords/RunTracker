using Microsoft.Extensions.Configuration;
using RunTracker.Command;
using RunTracker.Model;
using RunTracker.Repository;
using RunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RunTracker.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private IRunningSessionRepository _runningSessionRepository;

        private IRunTypeRepository _runTypeRepository;

        private readonly IConfiguration? _configuration;

        private DatabaseService _databaseService;

        public ObservableCollection<RunningSession> RunningSessions { get; set; } = new ObservableCollection<RunningSession>();

        public ObservableCollection<RunType> RunTypes { get; set; } = new ObservableCollection<RunType>();

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
        public RunType RunType { get; set; }

        private string _runTypeNew;
        public string RunTypeNew
        {
            get { return _runTypeNew; }
            set
            {
                if (_runTypeNew != value)
                {
                    _runTypeNew = value;
                    RaisePropertyChanged(); 
                }
            }
        }

        public DelegateCommand AddRunningSessionCommand { get; }
        public DelegateCommand UpdateRunningSessionCommand { get; }
        public DelegateCommand DeleteRunningSessionCommand { get; }

        public DelegateCommand AddRunTypeCommand { get; }

        //public bool CanAddRun => !double.IsNaN(Distance) && Time != TimeSpan.Zero && !string.IsNullOrEmpty(RunType) && Date.HasValue;

        //public bool CanUpdateRun => SelectedRunningSession != null;
        //public bool CanDeleteRun => SelectedRunningSession != null;

        public MainWindowViewModel(IConfiguration? configuration)
        {
            _configuration = configuration;

            AddRunningSessionCommand = new DelegateCommand(async _ => await AddRunningSessionAsync());
            UpdateRunningSessionCommand = new DelegateCommand(async _ => await UpdateRunningSessionAsync());
            DeleteRunningSessionCommand = new DelegateCommand(async _ => await DeleteRunningSessionAsync());
            AddRunTypeCommand = new DelegateCommand(async _ => await AddRunTypeAsync());

            ConnectToDatabase();
            LoadRunningSessions();
            LoadRunTypes();
        }


        private void ConnectToDatabase()
        {
            _databaseService = new DatabaseService(_configuration);
            _runningSessionRepository = new RunningSessionRepository(_databaseService.Database);
            _runTypeRepository = new RunTypeRepository(_databaseService.Database);
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

        private async Task LoadRunTypes()
        {
            var types = await _runTypeRepository.GetAllAsync();
            RunTypes.Clear();
            foreach (var type in types)
            {
                RunTypes.Add(type);
            }
        }

        private async Task AddRunningSessionAsync()
        {
            if (RunType == null)
            {
                MessageBox.Show("Vänligen välj en typ av löppass.");
                return;
            }

            var newSession = new RunningSession
            {
                Date = (DateTime)Date,
                Distance = Distance,
                Time = Time,
                RunType = RunType
            };
            await _runningSessionRepository.AddAsync(newSession);
            await LoadRunningSessions();
        }

        private async Task UpdateRunningSessionAsync()
        {
            if (SelectedRunningSession != null)
            {
                SelectedRunningSession.Date = (DateTime)Date;
                SelectedRunningSession.Distance = Distance;
                SelectedRunningSession.Time = Time;
                SelectedRunningSession.RunType = RunType;

                await _runningSessionRepository.UpdateAsync(SelectedRunningSession);

                await LoadRunningSessions();
            }
        }

        private async Task DeleteRunningSessionAsync()
        {
            if (SelectedRunningSession != null)
            {
                await _runningSessionRepository.DeleteAsync(SelectedRunningSession.Id);
                RunningSessions.Remove(SelectedRunningSession);
            }
        }

        private async Task AddRunTypeAsync()
        {
            var newType = new RunType
            {
                Name = RunTypeNew
            };
            await _runTypeRepository.AddAsync(newType);

            RunTypeNew = string.Empty;

            await LoadRunTypes();

        }

    }
}
