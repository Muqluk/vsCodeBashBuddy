using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System;

namespace vsCodeBashBuddy.ViewModel {
  public class ViewModelLocator {
    AppViewModelRegistry registry = AppViewModelRegistry.getRegistry;

    #region Properties

    public List<IViewModelBase> ViewModels { get { return registry.ViewModels; } }

    #endregion

    public ViewModelLocator() {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
      Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IViewModelBase)) && !t.IsAbstract)
        .ToList().ForEach(vm => {
          var instance = Activator.CreateInstance(vm) as IViewModelBase;
          SimpleIoc.Default.Register<IViewModelBase>(() => vm as IViewModelBase, vm.Name);
          registry.ViewModels.Add(instance);
        });
    }

    public void Cleanup() {
      registry.DisposeAllThreads();
    }
  }
}