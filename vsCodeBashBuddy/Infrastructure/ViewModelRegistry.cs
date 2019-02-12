using System;
using System.Collections.Generic;

namespace vsCodeBashBuddy.ViewModel {
  public class AppViewModelRegistry : IViewModelRegistry {

    private static AppViewModelRegistry _registry = null;


    #region Properties

    public static AppViewModelRegistry getRegistry {
      get {
        if (_registry == null) {
          _registry = new AppViewModelRegistry();
        }
        return _registry;
      }
    }

    public List<IViewModelBase> ViewModels { get; set; }

    #endregion

    #region CTOR

    private AppViewModelRegistry() {
      ViewModels = new List<IViewModelBase>();
    }

    #endregion

    public void DisposeAllThreads() {
      List<KeyValuePair<string, string>> threadsDisposeFails = new List<KeyValuePair<string, string>>();
      var name = string.Empty;
      var error = string.Empty;
      ViewModels.ForEach(vm => {
        name = string.Empty;
        error = string.Empty;
        try {
          vm.DisposeThreads();
        } catch (Exception ex) {
          error = ex.Message;
          threadsDisposeFails.Add(new KeyValuePair<string, string>(name, error));
        }
      });

      if (threadsDisposeFails.Count > 0) {
        // should be a string builder not concatenated string.
        string message = "Some threads failed to end as expected: \n";
        threadsDisposeFails.ForEach(f => {
          message += f.Key.PadRight(15);
          message += f.Value.PadLeft(75);
          message += "\n";
        });
        System.Windows.MessageBox.Show(message);
      }
    }

    public void CloseRequest() {
      ViewModels.ForEach(vm => vm.DisposeThreads());
    }
  }
}
