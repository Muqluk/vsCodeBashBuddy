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
    private bool _killButtonEnabled = false;
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
          if (_autoRefreshApps) {
            StartAppWatcher();
            _requestStopRefreshApps = false;
            ReloadAppsEnabled = false;
          } else {
            requestRefreshAppsStop();
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
      appsWatcher = new Thread(new ThreadStart(ReloadWatchedApps));
    }
    private void AppWatcher() {
      while (_autoRefreshApps) {
        ReloadWatchedApps();
        Thread.Sleep(500);
      }
    }

    #endregion

    #region Command Handlers
    private void ReloadWatchedApps() {

      updateEnableReloadAppsBtn(false);
      var processes = Process.GetProcesses();
      var apps = Enumerable.Empty<string>();
      ReloadAppsEnabled = false;
      KillButtonEnabled = false;

      foreach (var proc in processes) {
        if (watchedApps.Contains(proc.ProcessName)) {
          apps = apps.Concat<string>(new [] { proc.ProcessName });
        }
      }

      WatchedAppList = apps.Distinct().OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase);

      updateEnableReloadAppsBtn(true);
      KillButtonEnabled = true;
    }

    private void KillSelectedApps() {
      updateEnableReloadAppsBtn(false);
      KillButtonEnabled = false;
      var processes = Process.GetProcesses();
      var apps = Enumerable.Empty<string>();
      updateEnableReloadAppsBtn(false);
      KillButtonEnabled = false;

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

      updateEnableReloadAppsBtn(true);
      KillButtonEnabled = true;

    }
    #endregion

    #region Application Event Handlers

    private void HandleWindowClosing() {
      int attempts = 0;
      while (_autoRefreshApps || appsWatcher.IsAlive || attempts >= 50) {
        requestRefreshAppsStop();
        Thread.Sleep(50);
      }
      if (attempts >= 50) { // hard kill it.
        appsWatcher.Abort();
      }
    }

    #endregion

    private void requestRefreshAppsStop() {
      this.AutoRefreshApps = false;
    }

    private void updateEnableReloadAppsBtn(bool enableReloadButton) {
      ReloadAppsEnabled = (!_autoRefreshApps && enableReloadButton)
        ? true
        : false;
    }
  }
}