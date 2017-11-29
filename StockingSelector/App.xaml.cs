using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using log4net;
using log4net.Core;
using StockingSelector.Utility;
using StockingSelector.View.Window;
using StockingSelector.ViewModel;

namespace StockingSelector
{
  /// <summary>
  /// Object containing the main lifecycle logic of the application, responsible for initializing/
  /// tearing down relevant state (such as instantiating logging and 
  /// </summary>
  public partial class App : Application
  {
    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();

    #endregion

    #region Properties

    /// <summary>
    /// The name of the currently-executing application
    /// </summary>
    public static string Name { get; } = Assembly.GetEntryAssembly().GetName().Name;


    /// <summary>
    /// The version number of the currently-executing application
    /// </summary>
    public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

    #endregion

    #region Protected Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="args"></param>
    protected override void OnStartup(StartupEventArgs args)
    {
      base.OnStartup(args);

      // Initialize the logger for the application
      if (!LoggingUtilities.InitializeLogger())
      {
        DoShutdown(ResultCode.LoggerFailure);
        return;
      }

      // Hook up an exception handler for processing unhandled exceptions thrown on the dispatcher thread
      DispatcherUnhandledException += DispatcherExceptionHandler;
      
      try
      {
        var viewmodel = new MainViewModel();
        var mainWindow = new MainWindow
        {
          DataContext = viewmodel,
        };

        mainWindow.Show();
      }
      catch (Exception ex)
      {
        if (Logger.IsErrorEnabled)
          Logger.Error("An exception occurred during startup", ex);
        DoShutdown(ResultCode.LifecycleFailure);
      }
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="args"></param>
    protected override void OnExit(ExitEventArgs args)
    {
      if (!LoggingUtilities.ShutdownLogger())
      {
        DoShutdown(ResultCode.LoggerFailure);
        return;
      }

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
      if (Logger.IsErrorEnabled)
        Logger.Error("An unhandled exception occurred on the dispatcher thread", args.Exception);
      DoShutdown(ResultCode.UnhandledDispatcherException);
    }


    /// <summary>
    /// @Document
    /// </summary>
    private void DoShutdown(ResultCode code = ResultCode.Success)
    {
      Logger.Logger.Log(new LoggingEvent(new LoggingEventData
      {
        Level = code.IsError() ? Level.Warn : Level.Info,
        Message = $"Application shutting down with code {code.ToInt()} ({code})",
      }));

      Shutdown(code.ToInt());
    }

    #endregion
  }
}
