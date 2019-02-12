using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;


namespace vsCodeBashBuddy.ViewModel {
  public class MainViewModel : ViewModelBase<IViewModelRegistry> {

    private IViewModelRegistry _registry;

    public ObservableCollection<UserControl> Controls { get; set; }

    public RelayCommand HandleWindowClosingCmd {
      get { return new RelayCommand(HandleWindowClosing, () => true); }
    }

    public RelayCommand CheckDisposeThreadsRegistryCommand {
      get { return new RelayCommand(HandleCheckRegistration, () => true); }
    }

    public MainViewModel(IViewModelRegistry registry) : base(registry) {
      _registry = registry;
      //var viewModels = (App.Current.Resources["Locator"] as ViewModel.ViewModelLocator).ViewModels.FindAll(c => c.Name != "Main");
      //Controls = new ObservableCollection<UserControl>();
      //Assembly.GetExecutingAssembly()
      //  .GetTypes()
      //  //.Where(t => {
      //  //  return t.GetInterfaces().Contains(typeof(UserControl));
      //  //})
      //  .ToList()
      //  .ForEach(view => {
      //    if (view.IsSubclassOf(typeof(UserControl))) {
      //      //Controls.Add(view. as UserControl);
      //      var a = 1;
      //      var instance = Activator.CreateInstance(view) as UserControl;
      //      Controls.Add(instance);
      //    }
      //    //Controls.Add(instance);
      //  });
      var a = 1;
    }


    #region Application Event Handlers

    // perform all application thread clean up.
    private void HandleWindowClosing() {
      //int attempts = 0;
      _registry.CloseRequest();

    }

    private void HandleCheckRegistration() {

    }

    #endregion

    #region base overrides
    public override void RegisterThreads() { }
    public override void DisposeThreads() { }
    #endregion
  }
}