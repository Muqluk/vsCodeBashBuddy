using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace vsCodeBashBuddy.ViewModel.Messaging {
  public class AppCloseRequest {
    public bool RequestClose { get; set; }
  }
}
