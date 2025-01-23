using Microsoft.Extensions.Configuration;
using RunTracker.Command;
using RunTracker.Dialog;
using RunTracker.Model;
using RunTracker.Repository;
using RunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace RunTracker.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IRunningSessionRepository _runningSessionRepository;

        private IRunTypeRepository _runTypeRepository;

        private readonly IConfiguration? _configuration;

        private DatabaseService _databaseService;

        public ObservableCollection<RunningSession> RunningSessions { get; set; } = new ObservableCollection<RunningSession>();

        public ObservableCollection<RunType> RunTypes { get; set; } = new ObservableCollection<RunType>();


        public RunningSession _runningSession;

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

        public RunType _runType;

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
        
        public DelegateCommand DeleteRunningSessionCommand { get; }

        public DelegateCommand AddRunTypeCommand { get; }

        public DelegateCommand DeleteRunTypeCommand { get; }

        public DelegateCommand OpenEditRunningSessionDialogCommand { get; }

        public DelegateCommand OpenEditRunTypeDialogCommand { get; }

        public MainWindowViewModel(IConfiguration? configuration)
        {
            _configuration = configuration;

            RunningSession = new RunningSession();

            AddRunningSessionCommand = new DelegateCommand(async _ => await AddRunningSessionAsync());
            
            DeleteRunningSessionCommand = new DelegateCommand(async (parameter) => await DeleteRunningSessionAsync(parameter as RunningSession));
            
            AddRunTypeCommand = new DelegateCommand(async _ => await AddRunTypeAsync());

            DeleteRunTypeCommand = new DelegateCommand(async (parameter) => await DeleteRunTypeAsync(parameter as RunType));

            OpenEditRunningSessionDialogCommand = new DelegateCommand(async (parameter) => OpenEditRunningSessionDialog(parameter as RunningSession));
            
            OpenEditRunTypeDialogCommand = new DelegateCommand(async (parameter) => OpenEditRunTypeDialog(parameter as RunType));
            
            ConnectToDatabase();
            InitializeDefaultRunTypes();
            
        }


        private void ConnectToDatabase()
        {
            _databaseService = new DatabaseService(_configuration);
            _runningSessionRepository = new RunningSessionRepository(_databaseService.Database);
            _runTypeRepository = new RunTypeRepository(_databaseService.Database);
        }

        private async Task InitializeDefaultRunTypes()
        {
            var existingRunTypes = await _runTypeRepository.GetAllAsync();
            if (!existingRunTypes.Any())
            {
                var trainingRunType = new RunType { Name = "Training" };
                var raceRunType = new RunType { Name = "Race" };
                await _runTypeRepository.AddAsync(trainingRunType);
                await _runTypeRepository.AddAsync(raceRunType);
            }

            await LoadRunningSessions();
            await LoadRunTypes();
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
                MessageBox.Show("All fields must be filled in.");
                return;
            }

            var newSession = new RunningSession
            {
                Date = (DateTime)RunningSession.Date,
                Distance = (decimal)RunningSession.Distance,
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
                MessageBox.Show($"An error occurred while saving the running session: {ex.Message}");
            }
        }

        public void OpenEditRunningSessionDialog(RunningSession session)
        {
            var selectedSession = session as RunningSession;

            var dialog = new EditRunningSessionDialog(selectedSession, RunTypes, _runningSessionRepository, async () => await LoadRunningSessions());
            dialog.ShowDialog();
        }

        private async Task DeleteRunningSessionAsync(RunningSession session)
        {
            if (session != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this Running Session?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _runningSessionRepository.DeleteAsync(session.Id);
                        RunningSessions.Remove(session);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fel vid radering: {ex.Message}");
                    }
                }
            }
        }


        private async Task AddRunTypeAsync()
        {
            if (string.IsNullOrWhiteSpace(RunTypeNew))
            {
                MessageBox.Show("The field cannot be empty.");
                return;
            }

            var newType = new RunType
            {
                Name = RunTypeNew
            };
            await _runTypeRepository.AddAsync(newType);

            RunTypeNew = string.Empty;

            await LoadRunTypes();

        }


        private void OpenEditRunTypeDialog(RunType? runType)
        {
            var selectedRunType = runType as RunType;

            var dialog = new EditRunTypeDialog(selectedRunType, _runTypeRepository);
            dialog.ShowDialog();
        }

        private async Task DeleteRunTypeAsync(RunType? runType)
        {
            if (runType != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this RunType?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _runTypeRepository.DeleteAsync(runType.Id);
                        RunTypes.Remove(runType);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during deletion: {ex.Message}");
                    }
                }
            }
        }

    }



}
