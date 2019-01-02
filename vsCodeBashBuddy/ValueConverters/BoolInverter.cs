﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace vsCodeBashBuddy.ValueConverters {
  [ValueConversion(typeof(bool), typeof(bool))]
  public class InverseVisibilityConverter : IValueConverter {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture) {
      if (targetType != typeof(bool))
        throw new InvalidOperationException("The target must be a boolean");

      return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture) {
      throw new NotSupportedException();
    }

    #endregion
  }
}
