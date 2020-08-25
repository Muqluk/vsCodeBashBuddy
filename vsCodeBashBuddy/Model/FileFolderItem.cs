using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

namespace vsCodeBashBuddy.Model {

  public enum FileFolderType {
    Directory,
    File,
    Root,
  }

  public interface IFileFolderItem {
    string Name { get; set; }
    string Path { get; set; }

    bool isLoaded { get; set; }
    FileFolderType FileFolderType { get; set; }

    ObservableCollection<IFileFolderItem> Descendents { get; set; }
    //int ChildCount { get; set; }
    long Size { get; set; }
    string Extension { get; set; }

    //int TotalChildCount { get; set; }
    //int TotalSize { get; set; }
    //ObservableCollection<IFileFolderItem> TotalDescendents { get; set; }
  }

  public class FileFolderItem : ViewModelBase, IFileFolderItem {
    private string _name = "";
    public string Name {
      get {
        return _name;
      }
      set {
        if (value != _name) {
          _name = value;
          RaisePropertyChanged(nameof(Name));
        }
      }
    }
    public string Path { get; set; }
    public bool isLoaded { get; set; }
    public FileFolderType FileFolderType { get; set; }

    public ObservableCollection<IFileFolderItem> Descendents {
      get;
      set;
    }
    public long Size { get; set; }
    public string Extension { get; set; }
  }
  //public int ChildCount { get; set; }
  //public int TotalChildCount { get; set; }
  //public int TotalSize { get; set; }
  //public ObservableCollection<IFileFolderItem> TotalDescendents { get; set; }

}
