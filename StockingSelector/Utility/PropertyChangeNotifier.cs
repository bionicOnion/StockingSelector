﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace StockingSelector.Utility
{
  /// <inheritdoc />
  /// <summary>
  /// @Document
  /// </summary>
  public class PropertyChangeNotifier : INotifyPropertyChanged
  {
    #region Events

    /// <inheritdoc />
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Protected Functions

    /// <summary>
    /// @Document
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool NotifyPropertyChanged(string propertyName)
    {
      try
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
      }
      catch (Exception ex)
      {
        // @Unimplemented add logging capabilities
        Console.WriteLine(ex);
        return false;
      }
    }


    /// <summary>
    /// @Document
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool NotifyPropertyChanged<T>(Expression<Func<T>> property)
    {
      return property?.Body is MemberExpression expression && NotifyPropertyChanged(expression.Member.Name);
    }


    /// <summary>
    /// @Document
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool NotifyPropertiesChanged(IEnumerable<string> propertyNames)
    {
      return propertyNames != null && NotifyPropertiesChanged(propertyNames.ToArray());
    }


    /// <summary>
    /// @Document
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool NotifyPropertiesChanged(params string[] propertyNames)
    {
      return propertyNames?.All(NotifyPropertyChanged) ?? false;
    }


    /// <summary>
    /// @Document
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool NotifyAllPropertiesChanged()
    {
      try
      {
        // Invoking the PropertyChanged event with an empty string is treated as an indication that
        // all properties on an object have been modified and should be refreshed.
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        return true;
      }
      catch (Exception ex)
      {
        // @Unimplemented add logging capabilities
        Console.WriteLine(ex);
        return false;
      }
    }

    #endregion
  }
}
