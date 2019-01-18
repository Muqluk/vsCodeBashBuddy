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
    }

    #endregion

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion
  }
}