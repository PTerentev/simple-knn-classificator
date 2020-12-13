using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KNN.Common;
using KNN.Infrastructure.Algorithm;
using KNN.Infrastructure.Parsing;
using KNN.Models.Input;
using Microsoft.Win32;

namespace KNN.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private const string ProcessingMessage = "Processing";

        private string filesDirectory;
        private string outputClassName;
        private string testPercentage;
        private bool canRunTest;

        private List<WiltEntity> wiltTrainingEntities;
        private List<WiltEntity> wiltTestEntities;

        public MainWindowViewModel()
        {
            InitializeCommands();
            WiltEntity = new WiltEntity();
            canRunTest = true;
        }

        public WiltEntity WiltEntity { get; set; }

        public bool NormalizationRequired { get; set; } = false;

        public int NeighborCount { get; set; } = 3;

        public bool CanRunTest
        {
            get
            {
                return canRunTest;
            }
            set
            {
                canRunTest = value;
                OnPropertyChanged();
            }
        }

        public string OutputClassName
        {
            get
            {
                return outputClassName;
            }
            set
            {
                outputClassName = value;
                OnPropertyChanged();
            }
        }

        public string TestPercentage
        {
            get
            {
                return testPercentage;
            }
            set
            {
                testPercentage = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetClassEntityCommand { get; private set; }

        public ICommand TestPercentageCommand { get; private set; }

        public ICommand LoadTrainingDataSetCommand { get; private set; }

        public ICommand LoadTestDataSetCommand { get; private set; }

        private void InitializeCommands()
        {
            GetClassEntityCommand = new RelayCommand(GetClassEntity, obj => wiltTrainingEntities != null);
            TestPercentageCommand = new RelayCommand(GetTestPercentage, obj => wiltTrainingEntities != null && wiltTestEntities != null);
            LoadTrainingDataSetCommand = new RelayCommand(obj => LoadDataSet(ref wiltTrainingEntities));
            LoadTestDataSetCommand = new RelayCommand(obj => LoadDataSet(ref wiltTestEntities), obj => wiltTrainingEntities != null);
        }

        private async void GetClassEntity(object obj)
        {
            await Task.Factory.StartNew(() =>
            {
                CanRunTest = false;
                OutputClassName = ProcessingMessage;
                var classificationService = new WiltClassificationService(wiltTrainingEntities, NormalizationRequired);
                var classEnum = classificationService.GetWiltEntityClass(WiltEntity, NeighborCount);
                OutputClassName = classEnum.ToString();
                CanRunTest = true;
            });
        }

        private async void GetTestPercentage(object obj)
        {
            await Task.Factory.StartNew(() =>
            {
                CanRunTest = false;
                TestPercentage = ProcessingMessage;
                var classificationService = new WiltClassificationService(wiltTrainingEntities, NormalizationRequired);
                var percentage = classificationService.GetTestPercentage(wiltTestEntities, NeighborCount);
                TestPercentage = percentage.ToString("N2") + " %";
                CanRunTest = true;
            });
        }

        private void LoadDataSet(ref List<WiltEntity> dataSet)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = string.IsNullOrEmpty(filesDirectory) ?
                AppDomain.CurrentDomain.BaseDirectory :
                filesDirectory;

            openFileDialog.Filter = "csv files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                try
                {
                    filesDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                    dataSet = WiltDataSetParser.ParseEntities(openFileDialog.FileName).ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
