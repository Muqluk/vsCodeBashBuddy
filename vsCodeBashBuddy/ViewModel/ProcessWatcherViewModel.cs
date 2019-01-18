using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace vsCodeBashBuddy.ViewModel {
  public class ProcessWatcherViewModel : ViewModelBase {
    #region statics
    static string [] watchedApps = { "bash", "cmd", "conhost", "git - bash", "mintty", "mongod", "node" };
    static string [] browsers = { "iexplore", "chrome", "firefox" };

    static string [] nuisanceApps = { };
    #endregion

    #region members

    private Thread appsWatcher;
    private bool _autoRefreshApps = false;
    private bool _includeBrowser = false;
    private bool _requestStopRefreshApps = false;
    private bool _reloadAppsEnabled = true;
    private bool _killButtonEnabled = true;
    private bool _displayingErrors = false;
    private bool _includeNuisanceApps = false;
    private IEnumerable<string> _watchedAppList;
    private IEnumerable<string> _errorsList;
    private string [] _currentWatchList = watchedApps;

    #endregion

    #region Properties

    private bool RequestStopRefreshApps {
      get {
        return _requestStopRefreshApps;
      }
      set {
        if (value != _requestStopRefreshApps) {
          _requestStopRefreshApps = value;
        }
      }
    }

    public bool AutoRefreshApps {
      get {
        return _autoRefreshApps;
      }
      set {
        if (value != _autoRefreshApps) {
          if (value == true) {
            System.Windows.MessageBox.Show("Reminder, currently threads are not stopped on application unload - so memory leak will exist.  Stop Threads manually.");
          }
          _autoRefreshApps = value;
          if (_autoRefreshApps == true) {
            _requestStopRefreshApps = false;
            StartAppWatcher();
            ReloadAppsEnabled = false;
          } else {
            _requestStopRefreshApps = true;
            //requestRefreshAppsStop();
          }
          RaisePropertyChanged("AutoRefreshApps");
        }
      }
    }

    public bool IncludeBrowser {
      get {
        return _includeBrowser;
      }
      set {
        if (value != _includeBrowser) {
          _includeBrowser = value;
          _currentWatchList = watchedApps;

          // my head hurts.  figger out how to merge 2 string arrays later - stupid thing to waste time on now.
          if (value) {
            var temp = _currentWatchList.ToList();
            temp.AddRange(browsers.ToList());
            _currentWatchList = temp.ToArray();
          }

          if (!AutoRefreshApps) ReloadWatchedApps();

          RaisePropertyChanged("IncludeBrowser");
        }
      }
    }

    public bool ReloadAppsEnabled {
      get {
        return _reloadAppsEnabled;
      }
      set {
        if (value != _reloadAppsEnabled) {
          _reloadAppsEnabled = value;
          RaisePropertyChanged("ReloadAppsEnabled");
        }
      }
    }

    public bool KillButtonEnabled {
      get {
        return _killButtonEnabled;
      }
      set {
        if (value != _killButtonEnabled) {
          _killButtonEnabled = value;
          RaisePropertyChanged("KillButtonEnabled");
        }
      }
    }

    public bool DisplayingErrors {
      get { return _displayingErrors; }
      set {
        if (value != _displayingErrors) {
          _displayingErrors = value;
          RaisePropertyChanged("DisplayingErrors");
        }
      }
    }

    public bool IncludeNuisanceApps {
      get { return _includeNuisanceApps; }
      set {
        if (value != _includeNuisanceApps) {
          _includeNuisanceApps = value;
          RaisePropertyChanged("IncludeNuisanceApps");
        }
      }
    }

    public IEnumerable<string> WatchedAppList {
      get {
        return _watchedAppList;
      }
      private set {
        if (value != _watchedAppList) {
          _watchedAppList = value;
          RaisePropertyChanged("WatchedAppList");
        }
      }
    }

    public IEnumerable<string> ErrorsList {
      get { return _errorsList; }
      set {
        if (value != _errorsList) {
          _errorsList = value;
          RaisePropertyChanged("ErrorsList");
        }
      }
    }

    #endregion

    #region Commands

    public RelayCommand ReloadWatchedAppsCmd {
      get { return new RelayCommand(ReloadWatchedApps, () => true); }
    }

    public RelayCommand KillSelectedAppsCmd {
      get { return new RelayCommand(KillSelectedApps, () => true); }
    }

    public RelayCommand ToggleErrorPanel {
      get { return new RelayCommand(HandleToggleErrorPanelClick, () => true); }
    }

    #endregion

    #region Ctor
    public ProcessWatcherViewModel() {
      ReloadWatchedApps();
    }
    #endregion

    #region Threading

    private void StartAppWatcher() {
      _requestStopRefreshApps = false;
      appsWatcher = new Thread(new ThreadStart(AppWatcher));
      appsWatcher.Start();
    }
    private void AppWatcher() {

      ReloadAppsEnabled = false;
      while (_autoRefreshApps) {
        ReloadWatchedApps();
        Thread.Sleep(500);
      }
      _autoRefreshApps = false;
      ReloadAppsEnabled = true;
    }

    #endregion

    #region Command Handlers

    private void ReloadWatchedApps() {
      var processes = Process.GetProcesses();
      var apps = Enumerable.Empty<string>();

      try {
        foreach (var proc in processes) {
          if (_currentWatchList.Contains(proc.ProcessName)) {
            apps = apps.Concat<string>(new [] { proc.ProcessName });
          }
          if (IncludeBrowser) {
            if (browsers.Contains(proc.ProcessName)) {
              apps = apps.Concat<string>(new [] { proc.ProcessName });
            }
          }
          System.Diagnostics.Debug.WriteLine(proc);
        }
      } catch (Exception e) {
        System.Windows.MessageBox.Show(e.Message);
      }

      WatchedAppList = apps.Distinct().OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase);
    }

    private void KillSelectedApps() {
      try {
        var processes = Process.GetProcesses();
        var apps = Enumerable.Empty<string>();

        foreach (var proc in processes) {
          if (_currentWatchList.Contains(proc.ProcessName)) {
            try {
              proc.Kill();
            } catch (Exception ex) {
              ErrorsList = ErrorsList.Concat<string>(new [] { ex.Message });
            }
          }
        }

        if (!AutoRefreshApps) {
          ReloadWatchedApps();
        }
      } catch (Exception ex) {
        System.Windows.MessageBox.Show(ex.Message);
      }
    }

    private void HandleToggleErrorPanelClick() {
      DisplayingErrors = !_displayingErrors;
    }

    #endregion

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion

  }
}
