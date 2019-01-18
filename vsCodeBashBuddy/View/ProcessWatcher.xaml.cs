using System.Windows.Controls;

namespace vsCodeBashBuddy.View {
  public partial class ProcessWatcher : UserControl {
    public ProcessWatcher() {
      InitializeComponent();
      var viewModel = (App.Current.Resources["Locator"] as ViewModel.ViewModelLocator).ViewModels.Find(c => c.Name == "ProcessWatcher");
      this.DataContext = viewModel;
    }
  }
}
