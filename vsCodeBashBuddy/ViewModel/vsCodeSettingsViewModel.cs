using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

using GalaSoft.MvvmLight;


namespace vsCodeBashBuddy.ViewModel {
  public class vsCodeSettingsViewModel : ViewModelBase {
    const string startupPath = @"C:\repo";

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion

  }
}
