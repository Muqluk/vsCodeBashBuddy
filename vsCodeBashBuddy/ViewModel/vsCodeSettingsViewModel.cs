using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using vsCodeBashBuddy.Model;

namespace vsCodeBashBuddy.ViewModel {
  public class vsCodeSettingsViewModel : ViewModelBase {
    const string startupPath = @"C:\repo";

    private IFolder _currentRepository;
    public IFolder CurrentRepository {
      get {
        return _currentRepository;
      }
      set {
        if (value != _currentRepository) {
          _currentRepository = value;
          ActiveRepository = new ObservableCollection<IFolder>();
          foreach (var folder in _currentRepository.SubFolders) {
            ActiveRepository.Add(folder);
          }
          //ActiveRepository.Add(_currentRepository);
          RaisePropertyChanged("CurrentRepository");
        }
      }
    }

    private ObservableCollection<IFolder> _repositoriesRoot;
    public ObservableCollection<IFolder> RepositoriesRoot {
      get {
        return _repositoriesRoot;
      }
      set {
        if (value != _repositoriesRoot) {
          _repositoriesRoot = value;
          RaisePropertyChanged("RepositoriesRoot");
        }
      }
    }

    private ObservableCollection<IFolder> _activeRepository;
    public ObservableCollection<IFolder> ActiveRepository {
      get {
        return _activeRepository;
      }
      set {
        if (value != _activeRepository) {
          _activeRepository = value;
          RaisePropertyChanged("ActiveRepository");
        }
      }
    }

    public vsCodeSettingsViewModel() {
      RepositoriesRoot = new ObservableCollection<IFolder>();
      var dir = new DirectoryInfo(startupPath);
      foreach (var d in dir.GetDirectories()) {
        RepositoriesRoot.Add(new Folder(d));
      }
      // remove later.
      getProcesses();
    }


    #region Temporary Code

    private IList<RunningProcess> _currentProcesses;
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

    private void getProcesses() {
      var processes = Process.GetProcesses();
      var hash = new HashSet<string>();

      foreach (var p in processes) {
        hash.Add(p.ProcessName);
      }

      CurrentProcesses = new List<RunningProcess>();
      foreach (var p in hash.OrderBy(x => x)) {
        CurrentProcesses.Add(new RunningProcess { Name = p, instances = processes.Where(x => x.ProcessName == p).Count() });
      }
    }

    #endregion

  }

  public class RunningProcess {
    public string Name { get; set; }
    public int instances { get; set; }
  }
}
