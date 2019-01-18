using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System;

namespace vsCodeBashBuddy.ViewModel {
  public class ViewModelLocator {
    public List<IViewModelBase> ViewModels { get; set; }

    public ViewModelLocator() {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
      ViewModels = new List<IViewModelBase>();

      Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IViewModelBase)) && !t.IsAbstract)
        .ToList()
        .ForEach(vm => {
          var instance = Activator.CreateInstance(vm) as IViewModelBase;
          ViewModels.Add(instance);
          SimpleIoc.Default.Register<IViewModelBase>(() => vm as IViewModelBase, vm.Name);
        });
    }

    public static void Cleanup() {
      // call each module and dispose any currently running threads before exiting.
    }
  }
}