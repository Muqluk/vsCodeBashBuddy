using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;


/*
 
  https://www.technical-recipes.com/2017/using-hierarchicaldatatemplate-with-treeview-in-wpf-mvvm/

 */


namespace treeview.ViewModel {
  public class MainViewModel : ViewModelBase {

    #region members

    private string m_rootPath;

    private Folder _directoryItems;

    #endregion

    #region Properties

    public string RootPath {
      get {
        return m_rootPath;
      }
      set {
        if (value != m_rootPath) {
          m_rootPath = value;
          RaisePropertyChanged("RootPath");
        }
      }
    }

    public Folder DirectoryItems {
      get {
        return _directoryItems;
      }
      set {
        if (value != _directoryItems) {
          _directoryItems = value;
          RaisePropertyChanged("DirectoryItems");
        }
      }
    }
    #endregion

    #region Commands

    public RelayCommand onBrowseFolderClick {
      get { return new RelayCommand(browseFolderClick, () => true); }
    }


    public RelayCommand<Object> onItemSelectedCommand {
      get { return new RelayCommand<Object>((args) => getSelected(args)); }
    }

    #endregion

    #region Command Handlers

    private void browseFolderClick() {
      var dlg = new FolderBrowserDialog();
      dlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
      dlg.ShowDialog();
      RootPath = dlg.SelectedPath;
      DirectoryItems = new Folder(DirectoryItems) { Name = RootPath };
      DirectoryItems.getSubFolders();
      RaisePropertyChanged("DirectoryItems");
    }

    private void getSelected(object args) {
      Folder folder = ((Folder)args);
      folder.getSubFolders();
    }

    #endregion
  }

  public class Folder : ObservableObject {
    private Folder parent;

    public Folder(Folder parent) {
      this.parent = parent;
    }

    #region members
    private string _name;
    private bool _isSelected = false;
    private ObservableCollection<Folder> _subfolders;
    #endregion

    #region Properties
    public string Name {
      get {
        return _name;
      }

      set {
        if (value != _name) {
          _name = value;
          //Set<string>(() => this.Name, ref _name, value);
          //RaisePropertyChanged("Name");

          this.notifier();
        }
      }
    }

    public ObservableCollection<Folder> subFolders {
      get {
        return _subfolders;
      }
      set {
        if (value != _subfolders) {
          _subfolders = value;
          //Set<ObservableCollection<Folder>>(() => this.subFolders, ref _subfolders, value);
          //RaisePropertyChanged("subFolders");
          this.notifier();
        }
      }
    }
    #endregion

    public void getSubFolders() {
      subFolders = new ObservableCollection<Folder>();
      Directory.GetDirectories(this.Name).ToList().ForEach(x => {
        System.Diagnostics.Debug.WriteLine(x);
        subFolders.Add(new Folder(this) { Name = x });
      });
    }

    public void notifier() {
      RaisePropertyChanged("Name");
      RaisePropertyChanged("subFolders");
      if (parent != null) parent.notifier();
    }
  }
}