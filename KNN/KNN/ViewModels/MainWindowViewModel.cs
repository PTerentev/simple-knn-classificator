using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private string outputClassName;
        private string testPercentage;

        private List<WiltEntity> wiltTrainingEntities;
        private List<WiltEntity> wiltTestEntities;

        public MainWindowViewModel()
        {
            InitializeCommands();
            WiltEntity = new WiltEntity();
        }

        public WiltEntity WiltEntity { get; set; }

        public bool NormalizationRequred { get; set; } = true;

        public int NeighbourCount { get; set; } = 3;

        public string OutputClassName
        {
            get
            {
                return outputClassName;
            }
            set
            {
                OnPropertyChanged();
                outputClassName = value;
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
                OnPropertyChanged();
                testPercentage = value;
            }
        }

        public ICommand GetClassEntityCommand { get; private set; }

        public ICommand TestPercentageCommand { get; private set; }

        public ICommand LoadTrainingDataSetCommand { get; private set; }

        public ICommand LoadTestDataSetCommand { get; private set; }

        private void InitializeCommands()
        {
            GetClassEntityCommand = new RelayCommand(GetClassEntity, (obj) => wiltTrainingEntities != null);
            TestPercentageCommand = new RelayCommand(GetTestPercentage, (obj) => wiltTrainingEntities != null && wiltTestEntities != null);
            LoadTrainingDataSetCommand = new RelayCommand(obj => LoadDataSet(ref wiltTrainingEntities));
            LoadTestDataSetCommand = new RelayCommand(obj => LoadDataSet(ref wiltTestEntities));
        }

        private void GetClassEntity(object obj)
        {
            OutputClassName = string.Empty;
            var classificationService = new WiltClassificationService(wiltTrainingEntities, NormalizationRequred);
            var classEnum = classificationService.GetWiltEntityClass(WiltEntity, NeighbourCount);
            OutputClassName = classEnum.ToString();
        }

        private void GetTestPercentage(object obj)
        {
            TestPercentage = string.Empty;
            var classificationService = new WiltClassificationService(wiltTrainingEntities, NormalizationRequred);
            var percentage = classificationService.GetTestPercentage(wiltTestEntities, NeighbourCount);
            TestPercentage = percentage.ToString();
        }

        private void LoadDataSet(ref List<WiltEntity> dataSet)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "csv files (*.csv)";

            if (openFileDialog.ShowDialog() == true)
            {
                dataSet = WiltDataSetParser.ParseEntities(openFileDialog.FileName).ToList();
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
