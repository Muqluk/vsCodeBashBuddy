using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace vsCodeBashBuddy.ViewModel {
  public class MainViewModel : ViewModelBase {
    #region statics
    string [] watchedApps = { "bash", "cmd", "conhost", "git - bash", "iexplore", "mintty", "mongod", "node", "chrome" };
    #endregion

    #region members

    private Thread appsWatcher;
    private bool _autoRefreshApps = false;
    private bool _requestStopRefreshApps = false;
    private bool _reloadAppsEnabled = true;
    private bool _killButtonEnabled = true;
    private IEnumerable<string> _watchedAppList;

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

    #endregion

    #region Commands
    // this one will go away later - should be a thread in the background auto watching.
    public RelayCommand ReloadWatchedAppsCmd {
      get { return new RelayCommand(ReloadWatchedApps, () => true); }
    }
    public RelayCommand KillSelectedAppsCmd {
      get { return new RelayCommand(KillSelectedApps, () => true); }
    }
    public RelayCommand HandleWindowClosingCmd {
      get { return new RelayCommand(HandleWindowClosing, () => true); }
    }
    #endregion

    #region Ctor
    public MainViewModel() {
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
          if (watchedApps.Contains(proc.ProcessName)) {
            apps = apps.Concat<string>(new [] { proc.ProcessName });
          }
        }
      } catch (Exception e) {

      }

      WatchedAppList = apps.Distinct().OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase);
    }

    private void KillSelectedApps() {
      var processes = Process.GetProcesses();
      var apps = Enumerable.Empty<string>();

      foreach (var proc in processes) {
        if (watchedApps.Contains(proc.ProcessName)) {
          try {
            proc.Kill();
          } catch {
            // do nothing.
          }
        }
      }

      if (!AutoRefreshApps) {
        ReloadWatchedApps();
      }
    }
    #endregion

    #region Application Event Handlers

    private void HandleWindowClosing() {
      int attempts = 0;
      if (appsWatcher != null) {
        while (_autoRefreshApps || appsWatcher.IsAlive || attempts >= 50) {
          this.AutoRefreshApps = false;
          Thread.Sleep(50);
        }
        if (attempts >= 5) { // hard kill it.
          appsWatcher.Abort();
        }
      }
    }

    #endregion
  }
}