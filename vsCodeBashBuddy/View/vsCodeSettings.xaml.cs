using System.Windows.Controls;

namespace vsCodeBashBuddy.View {
  public partial class vsCodeSettings : UserControl {
    public vsCodeSettings() {
      InitializeComponent();
      var viewModel = (App.Current.Resources["Locator"] as ViewModel.ViewModelLocator).ViewModels.Find(c => c.Name == "vsCodeSettings");
      this.DataContext = viewModel;
    }
  }
}
