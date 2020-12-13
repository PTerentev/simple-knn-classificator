using KNN.ViewModels;
using System.Windows;

namespace KNN.Views
{
    /// <summary>
    /// Interaction logic for MainWindowViewModel.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
