using System;

using MvvmViewModelBase = GalaSoft.MvvmLight.ViewModelBase;
using GalaSoft.MvvmLight.Command;

namespace vsCodeBashBuddy.ViewModel {
  public interface IViewModelBase {
    string Name { get; }
    void RegisterThreads();
    void DisposeThreads();
  }

  public abstract class ViewModelBase : MvvmViewModelBase, IViewModelBase {
    public string Name {
      get {
        return this.GetType().Name.Replace("ViewModel", "");
      }
    }
    public abstract void RegisterThreads();
    public abstract void DisposeThreads();
  }
}
