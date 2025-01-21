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
                        RunningSession.Date = _selectedRunningSession.Date;
                        RunningSession.Distance = _selectedRunningSession.Distance;
                        RunningSession.Time = _selectedRunningSession.Time;
                        RunningSession.RunType = _selectedRunningSession.RunType;

                        RaisePropertyChanged(nameof(RunningSession.Date));
                        RaisePropertyChanged(nameof(RunningSession.Distance));
                        RaisePropertyChanged(nameof(RunningSession.Time));
                        RaisePropertyChanged(nameof(RunningSession.RunType));
                    }
                }
            }
        }

        private RunningSession _runningSession;

        public RunningSession RunningSession
        {
            get { return _runningSession; }
            set 
            { 
                if (_runningSession != value) 
                {
                    _runningSession = value;
                    RaisePropertyChanged();
                } 
            }
        }

        private RunType _runType;

        public RunType RunType
        {
            get { return _runType; }
            set 
            { 
                if (_runType != value)
                {
                    _runType = value;
                    RaisePropertyChanged();
                }
            }
        }

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

            RunningSession = new RunningSession();

            AddRunningSessionCommand = new DelegateCommand(async _ => await AddRunningSessionAsync());
            UpdateRunningSessionCommand = new DelegateCommand(async _ => await UpdateRunningSessionAsync());
            DeleteRunningSessionCommand = new DelegateCommand(async (parameter) => await DeleteRunningSessionAsync(parameter as RunningSession));
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

            var sortedSessions = sessions.OrderByDescending(session => session.Date).ToList();

            RunningSessions.Clear();
            foreach (var session in sortedSessions)
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
            if (RunningSession.Date == null || RunningSession.Distance == null || RunningSession.Time == null || RunType == null)
            {
                MessageBox.Show("Alla fält måste fyllas i.");
                return;
            }

            var newSession = new RunningSession
            {
                Date = (DateTime)RunningSession.Date,
                Distance = (float)RunningSession.Distance,
                Time = (TimeSpan)RunningSession.Time,
                RunType = RunType
            };

            try
            {
                await _runningSessionRepository.AddAsync(newSession);

                RunningSession = new RunningSession();  
                RunType = null;  

                await LoadRunningSessions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod vid lagring av löppasset: {ex.Message}");
            }
        }



        private async Task UpdateRunningSessionAsync()
        {
            if (SelectedRunningSession != null)
            {
                SelectedRunningSession.Date = (DateTime)RunningSession.Date;
                SelectedRunningSession.Distance = (float)RunningSession.Distance;
                SelectedRunningSession.Time = (TimeSpan)RunningSession.Time;
                SelectedRunningSession.RunType = RunningSession.RunType;

                await _runningSessionRepository.UpdateAsync(SelectedRunningSession);

                await LoadRunningSessions();
            }
        }

        //private async Task DeleteRunningSessionAsync()
        //{
        //    if (SelectedRunningSession != null)
        //    {
        //        await _runningSessionRepository.DeleteAsync(SelectedRunningSession.Id);
        //        RunningSessions.Remove(SelectedRunningSession);
        //    }
        //}

        private async Task DeleteRunningSessionAsync(RunningSession session)
        {
            if (session != null)
            {
                try
                {
                    // Ta bort löppasset från databasen
                    await _runningSessionRepository.DeleteAsync(session.Id);

                    // Ta bort löppasset från listan
                    RunningSessions.Remove(session);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fel vid radering: {ex.Message}");
                }
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
