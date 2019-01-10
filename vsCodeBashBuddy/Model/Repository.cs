using System.Collections.ObjectModel;
using System.IO;

using GalaSoft.MvvmLight;

namespace vsCodeBashBuddy.Model {

  public interface IRepository {
    DirectoryInfo RootFolder { get; set; }
    ObservableCollection<IFolder> Folders { get; set; }
  }

  public class Repository : ViewModelBase, IRepository {
    public DirectoryInfo RootFolder { get; set; }

    private ObservableCollection<IFolder> _folders;
    public ObservableCollection<IFolder> Folders {
      get {
        if (_folders == null) {
          _folders = new ObservableCollection<IFolder>();
        }
        return _folders;
      }
      set {
        if (value != _folders) {
          _folders = value;
          RaisePropertyChanged("Folders");
        }
      }
    }

    public Repository() { }
    public Repository(DirectoryInfo dir) {
      foreach (var subdir in dir.GetDirectories()) {

      }
    }

  }
}
