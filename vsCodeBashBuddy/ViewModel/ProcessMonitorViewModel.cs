using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using GalaSoft.MvvmLight.Command;

namespace vsCodeBashBuddy.ViewModel {
  public class ProcessMonitorViewModel : ViewModelBase {

    #region members

    private IList<RunningProcess> _currentProcesses;

    #endregion

    #region Properties

    public IList<RunningProcess> CurrentProcesses {
      get {
        return _currentProcesses;
      }
      set {
        if (value != _currentProcesses) {
          _currentProcesses = value;
          RaisePropertyChanged("CurrentProcesses");
        }
      }
    }

    #endregion

    #region Commands

    public RelayCommand SortByInstances {
      get {
        return new RelayCommand(() => {
          CurrentProcesses = CurrentProcesses.OrderByDescending(x => x.instances).ToList();
        },
        () => true);
      }
    }

    #endregion

    #region Ctor

    public ProcessMonitorViewModel() {
      this.getProcesses();
    }

    #endregion

    #region private methods

    private void getProcesses() {
      var processes = Process.GetProcesses();
      var hash = new HashSet<string>();

      CurrentProcesses = processes.ToList()
        .GroupBy(p => p.ProcessName)
        .Select(pg => new RunningProcess {
          Name = pg.First().ProcessName,
          instances = pg.Count(),
          privateMemory = pg.Sum(sp => sp.PrivateMemorySize64) / 1024 / 1024,
        }).ToList();
    }

    #endregion

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion
  }
}
