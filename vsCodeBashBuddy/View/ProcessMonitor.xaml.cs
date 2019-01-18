using System.Windows.Controls;

namespace vsCodeBashBuddy.View {
  public partial class ProcessMonitor : UserControl {
    public ProcessMonitor() {
      InitializeComponent();
      var viewModel = (App.Current.Resources["Locator"] as ViewModel.ViewModelLocator).ViewModels.Find(c => c.Name == "ProcessMonitor");
      this.DataContext = viewModel;
    }
  }
}
