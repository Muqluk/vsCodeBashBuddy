using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;


namespace vsCodeBashBuddy.ViewModel {
  public class MainViewModel : ViewModelBase {

    public RelayCommand HandleWindowClosingCmd {
      get { return new RelayCommand(HandleWindowClosing, () => true); }
    }

    public MainViewModel() { }


    #region Application Event Handlers

    // perform all application thread clean up.
    private void HandleWindowClosing() {
      int attempts = 0;

      //if (appsWatcher != null) {
      //  while (_autoRefreshApps || appsWatcher.IsAlive || attempts >= 50) {
      //    this.AutoRefreshApps = false;
      //    Thread.Sleep(50);
      //  }
      //  if (attempts >= 5) { // hard kill it.
      //    appsWatcher.Abort();
      //  }
      //}
    }

    #endregion
  }
}