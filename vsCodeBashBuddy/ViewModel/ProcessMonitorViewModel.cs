using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using vsCodeBashBuddy.Model;

namespace vsCodeBashBuddy.ViewModel {
  public class ProcessMonitorViewModel : ViewModelBase {

    enum SortByType {
      Asc = 0,
      Desc = 1,
      None = 2
    }

    private class Sorting {
      string _sortByColumn;
      string SortByColumn {
        get {
          return _sortByColumn;
        }
        set {
          if (value != _sortByColumn) {
            _sortByColumn = value;
            Sort = SortByType.Asc;
          }
        }
      }
      SortByType Sort { get; set; } = SortByType.None;
    }

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

    public RelayCommand SortByName {
      get {
        return new RelayCommand(() => {
          CurrentProcesses = CurrentProcesses.OrderByDescending(x => x.Name).ToList();
        },
        () => true);
      }
    }

    public RelayCommand SortByMemUsed {
      get {
        return new RelayCommand(() => {
          CurrentProcesses = CurrentProcesses.OrderByDescending(x => x.privateMemory).ToList();
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

      CurrentProcesses.OrderBy(p => p.Name).ToList().ForEach(p => Debug.WriteLine(p.Name));

    }

    #endregion

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion
  }
}
