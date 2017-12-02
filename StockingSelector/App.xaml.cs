using System;
using System.Net.Mail;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using log4net;
using log4net.Core;
using StockingSelector.Utility;
using StockingSelector.View.Windows;
using StockingSelector.ViewModel;

namespace StockingSelector
{
  /// <inheritdoc />
  /// <summary>
  /// Object containing the main lifecycle logic of the application, responsible for initializing/
  /// tearing down relevant state (such as instantiating logging and 
  /// </summary>
  public partial class App
  {
    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();


    /// <summary>
    /// @Document
    /// </summary>
    private MainViewModel _viewModel;

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


    /// <summary>
    /// The version number of the currently-executing application
    /// </summary>
    public static string Identifier { get; } = $"{Name} [v{Version}]";

    #endregion

    #region Protected Functions

    /// <inheritdoc />
    /// <summary>
    /// Hook for processing the start of the application's lifecycle to set up required state
    /// </summary>
    /// <param name="args">Arguments providing additional information about the starting of the application</param>
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
        // @Document
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        var loginViewModel = new LoginViewModel();
        new LoginWindow { DataContext = loginViewModel }.ShowDialog();
        if (!loginViewModel.DidUserLogin)
        {
          if (Logger.IsInfoEnabled)
            Logger.Info("User aborted login; exiting application");
          DoShutdown();
          return;
        }
        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

        if (Logger.IsInfoEnabled)
          Logger.InfoFormat("Logging in user with username {0}", loginViewModel.Username);

        // @Document
        _viewModel = new MainViewModel();
        if (!_viewModel.Initialize(new MailAddress(loginViewModel.Username), loginViewModel.Password))
        {
          if (Logger.IsWarnEnabled)
            Logger.Warn("Initialization of the backing viewmodel failed");
          DoShutdown(ResultCode.LifecycleFailure);
          return;
        }

        // @Document
        var mainWindow = new MainWindow
        {
          DataContext = _viewModel,
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


    /// <inheritdoc />
    /// <summary>
    /// Hook for processing the end of the application's lifecycle to tear down required state
    /// </summary>
    /// <param name="args">Arguments providing additional information about the closing of the application</param>
    protected override void OnExit(ExitEventArgs args)
    {
      // @Document
      _viewModel?.Dispose();
      _viewModel = null;

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
    /// Function installed as a handler for exceptions that go unhandled on the dispatcher thread. This function makes
    /// no attempt to resolve any issues that result from those exceptions, but instead serves to log out the failure
    /// and shut down the application cleanly.
    /// </summary>
    /// <param name="sender">The object that send the event notifying about the exception</param>
    /// <param name="args">Arguments containing information about the exception event</param>
    private void DispatcherExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
      if (Logger.IsErrorEnabled)
        Logger.Error("An unhandled exception occurred on the dispatcher thread", args.Exception);
      DoShutdown(ResultCode.UnhandledDispatcherException);
    }


    /// <summary>
    /// Wrapper for the Shutdown function that logs out information about the type of shutdown that the application is
    /// performing.
    /// </summary>
    /// <param name="code">The result code being returned by the application</param>
    private void DoShutdown(ResultCode code = ResultCode.Success)
    {
      // Marshal to the dispatcher thread if necessary
      if (!Dispatcher.CheckAccess())
      {
        Dispatcher.Invoke(() => DoShutdown(code));
        return;
      }

      // Log out the reason for shutting down
      var targetLevel = code.IsError() ? Level.Warn : Level.Info;
      if (Logger.Logger.IsEnabledFor(targetLevel))
        Logger.Logger.Log(GetType(), targetLevel, $"Application shutting down with code {code.ToInt()} ({code})", null);

      // Exit the application with the specified code
      Shutdown(code.ToInt());
    }

    #endregion
  }
}
