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
  [ValueConversion(typeof(bool), typeof(Visibility))]
  [Localizability(LocalizationCategory.NeverLocalize)]
  public class InverseVisibilityConverter : IValueConverter {
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

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is Visibility) {
        return (Visibility)value == Visibility.Visible;
      } else {
        return false;
      }
    }
  }

  [ValueConversion(typeof(bool), typeof(bool))]
  public class InverseBooleanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (targetType != typeof(bool)) {
        throw new InvalidOperationException("The target must be a boolean");
      }

      return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotSupportedException();
    }
  }
}
