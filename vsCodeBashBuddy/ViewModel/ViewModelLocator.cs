/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:vsCodeBashBuddy"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

namespace vsCodeBashBuddy.ViewModel {
  public class ViewModelLocator {
    public ViewModelLocator() {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      SimpleIoc.Default.Register<MainViewModel>();
      SimpleIoc.Default.Register<ProcessWatcherViewModel>();
      SimpleIoc.Default.Register<vsCodeSettingsViewModel>();
    }

    public MainViewModel Main {
      get {
        return ServiceLocator.Current.GetInstance<MainViewModel>();
      }
    }

    public ProcessWatcherViewModel ProcessWatcher {
      get {
        return ServiceLocator.Current.GetInstance<ProcessWatcherViewModel>();
      }
    }

    public vsCodeSettingsViewModel vsCodeSettings {
      get {
        return ServiceLocator.Current.GetInstance<vsCodeSettingsViewModel>();
      }
    }

    public static void Cleanup() {
      // TODO Clear the ViewModels
    }
  }
}