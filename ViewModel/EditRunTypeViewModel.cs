using RunTracker.Command;
using RunTracker.Model;
using RunTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RunTracker.ViewModel
{
    class EditRunTypeViewModel : ViewModelBase
    {
        private readonly IRunTypeRepository _runTypeRepository;

        private readonly Window _dialog;

        private RunType _selectedRunType;

        public RunType SelectedRunType
        {
            get => _selectedRunType;
            set
            {
                _selectedRunType = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateRunTypeCommand { get; }

        public EditRunTypeViewModel(RunType selectedRunType, IRunTypeRepository runTypeRepository, Window dialog)
        {
            SelectedRunType = selectedRunType;

            _runTypeRepository = runTypeRepository;
            _dialog = dialog;

            UpdateRunTypeCommand = new DelegateCommand(async _ => await UpdateRunTypeAsync());

        }

        private async Task UpdateRunTypeAsync()
        {
            try
            {
                await _runTypeRepository.UpdateAsync(SelectedRunType);
                MessageBox.Show("Typ av löppass har uppdaterats.");
                _dialog.Close(); // Stänger dialogrutan
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod vid uppdatering av typ av löppass: {ex.Message}");
            }
        }
    }
}
