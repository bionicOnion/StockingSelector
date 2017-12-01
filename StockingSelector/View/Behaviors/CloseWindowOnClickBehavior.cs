using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace StockingSelector.View.Behaviors
{
  /// <inheritdoc />
  /// <summary>
  /// @Document
  /// </summary>
  public class CloseWindowOnClickBehavior : Behavior<Button>
  {
    #region Constants

    /// <summary>
    /// @Document
    /// </summary>
    public static readonly DependencyProperty OnClosingCommandProperty =
      DependencyProperty.Register(nameof(OnClosingCommand), typeof(ICommand), typeof(CloseWindowOnClickBehavior));

    #endregion

    #region Properties

    /// <summary>
    /// @Document
    /// </summary>
    public ICommand OnClosingCommand
    {
      get => (ICommand) GetValue(OnClosingCommandProperty);
      set => SetValue(OnClosingCommandProperty, value);
    }

    #endregion

    #region Protected Functions

    /// <summary>
    /// @Document
    /// </summary>
    protected override void OnAttached()
    {
      base.OnAttached();

      AssociatedObject.Click += OnButtonClick;
    }


    /// <summary>
    /// @Document
    /// </summary>
    protected override void OnDetaching()
    {
      AssociatedObject.Click -= OnButtonClick;

      base.OnDetaching();
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// @Document
    /// </summary>
    private void OnButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
      // @Document
      if (!OnClosingCommand?.CanExecute(null) ?? false)
        return;

      // @Document
      OnClosingCommand?.Execute(null);

      // @Document
      var parentWindow = Window.GetWindow(AssociatedObject);
      parentWindow?.Close();
    }

    #endregion
  }
}
