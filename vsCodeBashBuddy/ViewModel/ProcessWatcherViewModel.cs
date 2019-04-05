using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using System.Windows;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace vsCodeBashBuddy.ViewModel {
  public class ProcessWatcherViewModel : ViewModelBase {
    #region statics -- I should be loading these in from the app.config
    static string[] watchedApps = { "bash", "cmd", "conhost", "git - bash", "mintty", "mongod", "node" };
    static string[] browsers = { "iexplore", "chrome", "firefox" };

    static string[] nuisanceApps = {
      "AppleMobileDeviceService",
      "GoogleCrashHandler",
      "GoogleCrashHandler64",
      "Microsoft.Photos",
      "OfficeClickToRun",
      "OfficeHubTaskHost",
      "SkypeApp",
      "SkypeBackgroundHost",
      "Teams"
    };
    #endregion

    #region members

    private Thread appsWatcher;
    private bool _autoRefreshApps = false;
    private bool _includeBrowser = false;
    private bool _requestStopRefreshApps = false;
    private bool _reloadAppsEnabled = true;
    private bool _killButtonEnabled = true;
    private bool _displayingErrors = false;
    private bool _watchNuisanceApps = false;
    private IEnumerable<string> _watchedAppList;
    private IEnumerable<string> _errorsList;
    private string[] _currentWatchList = watchedApps;

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
        //if (value == true) {
        //  MessageBox.Show("Not quite fully implemented dispose - so uncheck before exiting");
        //}
        if (value != _autoRefreshApps) {
          _autoRefreshApps = value;
          if (_autoRefreshApps == true) {
            _requestStopRefreshApps = false;
            StartAppWatcher();
            ReloadAppsEnabled = false;
          } else {
            _requestStopRefreshApps = true;
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
        if (!WatchNuisanceApps) {
          if (value != _includeBrowser) {
            _includeBrowser = value;
            _currentWatchList = watchedApps;

            // my head hurts.  figger out how to merge 2 string arrays later - stupid thing to waste time on now.
            if (value) {
              var temp = _currentWatchList.ToList();
              temp.AddRange(browsers.ToList());
              _currentWatchList = temp.ToArray();
            }

            if (!AutoRefreshApps) {
              ReloadWatchedApps();
            }

            RaisePropertyChanged("IncludeBrowser");
          }
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

    public bool WatchNuisanceApps {
      get { return _watchNuisanceApps; }
      set {
        if (value != _watchNuisanceApps) {
          if (value == true) {
            AutoRefreshApps = false;
            _currentWatchList = nuisanceApps;
            AutoRefreshApps = true;
          } else {
            AutoRefreshApps = false;
            _currentWatchList = watchedApps;
            AutoRefreshApps = true;
          }
          _watchNuisanceApps = value;
          RaisePropertyChanged("WatchNuisanceApps");
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

    public RelayCommand KillNuisanceAppsCmd {
      get { return new RelayCommand(KillNuisancesHandler, () => true); }
    }

    #endregion

    #region Ctor

    public ProcessWatcherViewModel() {
      Messenger.Default.Register<Messaging.AppCloseRequest>(this,
        (message) => {
          AutoRefreshApps = !message.RequestClose;
          this.RequestStopRefreshApps = message.RequestClose;
        });
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
      while (_autoRefreshApps && _requestStopRefreshApps == false) {
        ReloadWatchedApps();
        Thread.Sleep(500);
      }
      _autoRefreshApps = false;
      _requestStopRefreshApps = false;
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
            apps = apps.Concat(new[] { proc.ProcessName });
          }
          if (IncludeBrowser) {
            if (browsers.Contains(proc.ProcessName)) {
              apps = apps.Concat(new[] { proc.ProcessName });
            }
          }
        }
      } catch (Exception e) {
        MessageBox.Show(e.Message);
      }

      WatchedAppList = apps.Distinct().OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase);
    }

    private void HandleToggleErrorPanelClick() {
      DisplayingErrors = !_displayingErrors;
    }

    private void KillSelectedApps() {
      KillProcesses(_currentWatchList);
    }

    private void KillNuisancesHandler() {
      KillProcesses(nuisanceApps);
    }

    private void KillProcesses(string[] walkingDead) {
      try {
        var processes = Process.GetProcesses();
        var apps = Enumerable.Empty<string>();

        foreach (var proc in processes) {
          if (walkingDead.Contains(proc.ProcessName)) {
            try {
              proc.Kill();
            } catch (Exception ex) {
              ErrorsList = ErrorsList.Concat<string>(new[] { ex.Message });
            }
          }
          if (!AutoRefreshApps) {
            ReloadWatchedApps();
          }
        }
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    #endregion

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() {
      RequestStopRefreshApps = true;
      while (_autoRefreshApps) {
        Thread.Sleep(50);
      }
    }
    #endregion

  }
}
