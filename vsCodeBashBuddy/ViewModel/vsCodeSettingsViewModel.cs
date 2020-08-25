using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Navigation;

using GalaSoft.MvvmLight;

using vsCodeBashBuddy.Model;


namespace vsCodeBashBuddy.ViewModel {
  public class vsCodeSettingsViewModel : ViewModelBase {

    #region Private Members

    // add a directory browse dialog and button later.  For now, hard code.
    const string startupPath = @"C:\repo\gateway\ProOpt\ui-prooptimizer";

    #endregion

    #region properties

    private IFileFolderItem _currentRepository;
    public IFileFolderItem CurrentRepository {
      get {
        return _currentRepository;
      }
      set {
        if (value != _currentRepository) {
          _currentRepository = value;
          ActiveRepository = new ObservableCollection<IFileFolderItem>();
          foreach (var folder in _currentRepository.Descendents) {
            ActiveRepository.Add(folder);
          }
          RaisePropertyChanged("CurrentRepository");
        }
      }
    }

    private ObservableCollection<IFileFolderItem> _repositoriesRoot;
    public ObservableCollection<IFileFolderItem> RepositoriesRoot {
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

    private ObservableCollection<IFileFolderItem> _activeRepository;
    public ObservableCollection<IFileFolderItem> ActiveRepository {
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

    #endregion

    public vsCodeSettingsViewModel() {
      CurrentRepository = LoadDirectory(startupPath);
    }

    private IFileFolderItem LoadDirectory(string fromPath) {
      var dir = new DirectoryInfo(fromPath);
      IFileFolderItem ffItem = new FileFolderItem() {
        Name = dir.Name,
        Path = dir.FullName,
        isLoaded = true,
        FileFolderType = FileFolderType.Directory,
        Descendents = new ObservableCollection<IFileFolderItem>(),
      };

      foreach (var di in dir.GetDirectories()) {
        ffItem.Descendents.Add(LoadDirectory(di.FullName));
      }
      foreach (var fi in dir.GetFiles()) {
        ffItem.Descendents.Add(new FileFolderItem() {
          Name = fi.Name,
          Path = fi.FullName,
          FileFolderType = FileFolderType.File,
          Size = fi.Length,
          Extension = fi.Extension,
        });
      }

      return ffItem;
    }


    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion
  }
}
