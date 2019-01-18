using System.Windows;

namespace vsCodeBashBuddy {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      var viewModel = (App.Current.Resources["Locator"] as ViewModel.ViewModelLocator).ViewModels.Find(c => c.Name == "Main");
      this.DataContext = viewModel;
    }
  }
}
