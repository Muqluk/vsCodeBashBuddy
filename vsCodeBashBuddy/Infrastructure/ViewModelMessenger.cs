using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Messaging;

namespace vsCodeBashBuddy.ViewModel {
  public class ViewModelMessenger {
    public IViewModelBase ThreadHaltRequest { get; set; }
  }
}
