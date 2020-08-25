using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

using GalaSoft.MvvmLight;

using vsCodeBashBuddy.Model;


namespace vsCodeBashBuddy.ViewModel {
  public class vsCodeSettingsViewModel : ViewModelBase {
    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion

    // add a directory browse dialog and button later.  For now, hard code.
    const string startupPath = @"C:\repo\gateway\ProOpt\ui-prooptimizer";

    ObservableCollection<IFileFolderItem> Repository { get; set; }

  }
}
