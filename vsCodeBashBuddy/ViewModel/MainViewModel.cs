using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace vsCodeBashBuddy.ViewModel {
    public class MainViewModel : ViewModelBase {
        #region constants
        string [] watchedApps = { "bash", "cmd", "conhost", "git - bash", "iexplore", "mintty", "mongod", "node" };


        //"GoogleCrashHandler",
        //"GoogleCrashHandler64",
        //"GoogleUpdate",
        //"sh",
        //"ShellExperienceHost",
        //"sihost",
        //"SkypeApp",
        #endregion
        #region members
        private bool _reloadAppsEnabled = true;
        private bool _killButtonEnabled = false;
        private IEnumerable<string> _watchedAppList;
        #endregion

        #region Properties
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
        #endregion

        #region Ctor
        public MainViewModel() {
            ReloadWatchedApps();
        }
        #endregion

        #region Command Handlers
        private void ReloadWatchedApps() {
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

            ReloadAppsEnabled = true;
            KillButtonEnabled = true;
        }
        private void KillSelectedApps() {
            var processes = Process.GetProcesses();
            var apps = Enumerable.Empty<string>();
            ReloadAppsEnabled = false;
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

            ReloadWatchedApps();

            ReloadAppsEnabled = true;
            KillButtonEnabled = true;

        }
        #endregion

    }
}