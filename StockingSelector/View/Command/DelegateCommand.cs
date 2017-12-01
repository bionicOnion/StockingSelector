using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;

namespace StockingSelector.View.Command
{
  /// <inheritdoc />
  /// <summary>
  /// @Document
  /// </summary>
  public class DelegateCommand : ICommand
  {
    #region Constants

    /// <summary>
    /// @Document
    /// </summary>
    private const int True = 1;


    /// <summary>
    /// @Document
    /// </summary>
    private const int False = 0;

    #endregion

    #region Fields

    /// <summary>
    /// @Document
    /// </summary>
    private readonly Action<object> _executeDelegate;


    /// <summary>
    /// @Document
    /// </summary>
    private readonly Func<object, bool> _canExecuteDelegate;


    /// <summary>
    /// @Document
    /// </summary>
    private int _couldExecute = False;

    #endregion

    #region Events

    /// <inheritdoc />
    public event EventHandler CanExecuteChanged;

    #endregion

    #region Ctor

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="executeDelegate"></param>
    /// <param name="canExecuteDelegate"></param>
    public DelegateCommand(Action executeDelegate, Func<object, bool> canExecuteDelegate = null)
      : this(param => executeDelegate?.Invoke(), canExecuteDelegate)
    {
      // nop
    }

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="executeDelegate"></param>
    /// <param name="canExecuteDelegate"></param>
    public DelegateCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate = null)
    {
      _executeDelegate = executeDelegate;
      _canExecuteDelegate = canExecuteDelegate ?? (param => true);
    }

    #endregion

    #region Public Functions

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanExecute(object param)
    {
      var canExecute = _canExecuteDelegate?.Invoke(param) ?? true;
      var canExecuteInt = canExecute ? True : False;
      if (Interlocked.CompareExchange(ref _couldExecute, canExecuteInt, _couldExecute) != canExecuteInt)
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
      return canExecute;
    }


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(object param) => _executeDelegate?.Invoke(param);

    #endregion
  }
}
