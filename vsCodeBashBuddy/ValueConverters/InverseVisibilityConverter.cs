using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace vsCodeBashBuddy.ValueConverters {
  //[ValueConversion(typeof(bool), typeof(Visibility))]
  [Localizability(LocalizationCategory.NeverLocalize)]
  public class InverseVisibilityConverter : IValueConverter {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      bool bValue = false;
      if (value is bool) {
        bValue = (bool)value;
      } else if (value is Nullable<bool>) {
        Nullable<bool> tmp = (Nullable<bool>)value;
        bValue = tmp.HasValue ? tmp.Value : false;
      }
      return (bValue) ? Visibility.Collapsed : Visibility.Visible;

    }


    // this might be wrong.  just copy-pasta from Microsoft's reference-source for bool-to-vis converter.
    // https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/windows/Controls/BooleanToVisibilityConverter.cs
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is Visibility) {
        return (Visibility)value == Visibility.Visible;
      } else {
        return false;
      }
    }

    #endregion
  }
}
