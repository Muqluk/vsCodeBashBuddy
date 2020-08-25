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
    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion

    #region Private Members

    // add a directory browse dialog and button later.  For now, hard code.
    const string startupPath = @"C:\repo\gateway\ProOpt\ui-prooptimizer";

    #endregion

    #region Properties

    private IFileFolderItem _activeRepo = new FileFolderItem();
    protected IFileFolderItem ActiveRepository {
      get {
        return _activeRepo;
      }
      set {
        if (value != _activeRepo) {
          _activeRepo = value;
          RepoItems = value.Descendents.ToList();
          RaisePropertyChanged(nameof(ActiveRepository));
        }
      }
    }

    private List<IFileFolderItem> _repoItems;
    protected List<IFileFolderItem> RepoItems {
      get {
        return _repoItems;
      }
      set {
        if (value != _repoItems) {
          _repoItems = value;
          RaisePropertyChanged(nameof(RepoItems));
        }
      }
    }

    #endregion

    #region CTOR

    public vsCodeSettingsViewModel() {
      ActiveRepository = LoadDirectory(startupPath);
    }

    #endregion

    #region Private Methods

    private IFileFolderItem LoadDirectory(string fromPath) {
      IFileFolderItem ffItem = new FileFolderItem() {
        Name = Path.GetDirectoryName(fromPath),
        Path = fromPath,
        isLoaded = true,
        FileFolderType = FileFolderType.Root,
        Descendents = new ObservableCollection<IFileFolderItem>(),
      };

      foreach (var dir in Directory.GetDirectories(fromPath)) {
        var item = new FileFolderItem() {
          Name = Path.GetDirectoryName(dir),
          Path = dir,
          isLoaded = false,
          FileFolderType = FileFolderType.Directory,
          Descendents = new ObservableCollection<IFileFolderItem>(),
        };
        ffItem.Descendents.Add(item);
      }
      foreach (var file in Directory.GetFiles(fromPath)) {
        var fi = new FileInfo(file);
        var item = new FileFolderItem() {
          Name = fi.Name,
          Path = fi.FullName,
          FileFolderType = FileFolderType.File,
          Size = fi.Length,
          Extension = fi.Extension,
        };
        ffItem.Descendents.Add(item);
      }

      return ffItem;
    }

    #endregion

  }
}
