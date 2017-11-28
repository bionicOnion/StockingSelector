using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using StockingSelector.View.Window;
using StockingSelector.ViewModel;

namespace StockingSelector
{
  /// <summary>
  /// @Document
  /// </summary>
  public partial class App : Application
  {
    #region Properties

    /// <summary>
    /// @Document
    /// </summary>
    public static string Name { get; } = Assembly.GetEntryAssembly().GetName().Name;

    #endregion

    #region Protected Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="args"></param>
    protected override void OnStartup(StartupEventArgs args)
    {
      base.OnStartup(args);

      // @Document
      DispatcherUnhandledException += DispatcherExceptionHandler;

      try
      {
        var viewmodel = new StockingSelectorViewModel();
        var mainWindow = new StockingSelectorWindow
        {
          DataContext = viewmodel,
        };

        mainWindow.Show();
      }
      catch (Exception ex)
      {
        // @Unimplemented log this out
        Console.WriteLine(ex);
        Shutdown(-1); // @Unimplemented use a better error code
      }
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="args"></param>
    protected override void OnExit(ExitEventArgs args)
    {
      // @Unimplemented

      base.OnExit(args);
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void DispatcherExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
      // @Unimplemented Add logging, maybe a message box, and a more robust error code
      Shutdown(-1);
    }

    #endregion
  }
}
