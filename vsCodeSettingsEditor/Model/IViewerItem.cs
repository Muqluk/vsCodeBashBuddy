using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vsCodeSettingsEditor.Model {
  interface IViewerItem {
    string Name { get; set; }
    string Path { get; set; }

  }
}
