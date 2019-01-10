using System.Collections.ObjectModel;
using System.IO;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using vsCodeBashBuddy.Model;

namespace vsCodeBashBuddy.ViewModel {
  public class vsCodeSettingsViewModel : ViewModelBase {
    const string startupPath = @"C:\repo";

    //private IRepository _repositoryRoot;

    //public IRepository RepositoryRoot {
    //  get {
    //    return _repositoryRoot;
    //  }
    //  set {
    //    if (value != _repositoryRoot) {
    //      _repositoryRoot = value;
    //      RaisePropertyChanged("RepositoryRoot");
    //    }
    //  }
    //}


    private ObservableCollection<IFolder> _repositoryFolder;

    public ObservableCollection<IFolder> RepositoryFolder {
      get {
        return _repositoryFolder;
      }
      set {
        if (value != _repositoryFolder) {
          _repositoryFolder = value;
          RaisePropertyChanged("RepositoryFolder");
        }
      }
    }

    public vsCodeSettingsViewModel() {
      RepositoryFolder = new ObservableCollection<IFolder>();
      var dir = new DirectoryInfo(startupPath);
      foreach (var d in dir.GetDirectories()) {
        RepositoryFolder.Add(new Folder(d));
      }

    }
    

  }
}
