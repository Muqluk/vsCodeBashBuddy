using System.Collections.ObjectModel;
using System.IO;

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
          _currentRepository = null;
          RaisePropertyChanged("CurrentRepository");
          _currentRepository = value;
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

    public vsCodeSettingsViewModel() {
      RepositoriesRoot = new ObservableCollection<IFolder>();
      var dir = new DirectoryInfo(startupPath);
      foreach (var d in dir.GetDirectories()) {
        RepositoriesRoot.Add(new Folder(d));
      }
    }


  }
}
