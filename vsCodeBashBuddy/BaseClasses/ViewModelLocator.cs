using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System;

namespace vsCodeBashBuddy.ViewModel {
  public class AppViewModelRegistry {
    private static AppViewModelRegistry _registry = null;
    public static AppViewModelRegistry getRegistry {
      get {
        if (_registry == null) {
          _registry = new AppViewModelRegistry();
        }
        return _registry;
      }
    }

    private AppViewModelRegistry() {
      ViewModels = new List<IViewModelBase>();
    }

    public List<IViewModelBase> ViewModels { get; set; }

    public void DisposeAllThreads() {
      List<KeyValuePair<string, string>> threadsDisposeFails = new List<KeyValuePair<string, string>>();
      var name = string.Empty;
      var error = string.Empty;
      ViewModels.ForEach(vm => {
        name = string.Empty;
        error = string.Empty;
        try {
          vm.DisposeThreads();
        } catch (Exception ex) {
          error = ex.Message;
          threadsDisposeFails.Add(new KeyValuePair<string, string>(name, error));
        }
      });

      if (threadsDisposeFails.Count > 0) {
        // should be a string builder not concatenated string.
        string message = "Some threads failed to end as expected: \n";
        threadsDisposeFails.ForEach(f => {
          message += f.Key.PadRight(15);
          message += f.Value.PadLeft(75);
          message += "\n";
        });
        System.Windows.MessageBox.Show(message);
      }
    }
  }

  public class ViewModelLocator {
    public List<IViewModelBase> ViewModels {
      get {
        AppViewModelRegistry registry = AppViewModelRegistry.getRegistry;
        return registry.ViewModels;
      }
    }
    public ViewModelLocator() {
      AppViewModelRegistry registry = AppViewModelRegistry.getRegistry;
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IViewModelBase)) && !t.IsAbstract)
        .ToList().ForEach(vm => {
          var instance = Activator.CreateInstance(vm) as IViewModelBase;
          SimpleIoc.Default.Register<IViewModelBase>(() => vm as IViewModelBase, vm.Name);
          registry.ViewModels.Add(instance);
        });
    }

    public static void Cleanup() {
      AppViewModelRegistry registry = AppViewModelRegistry.getRegistry;
      // call each module and dispose any currently running threads before exiting.
      registry.DisposeAllThreads();
    }
  }
}