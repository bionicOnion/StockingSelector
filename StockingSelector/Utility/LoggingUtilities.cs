using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;

namespace StockingSelector.Utility
{
  /// <summary>
  /// @Document
  /// </summary>
  public static class LoggingUtilities
  {
    #region Constants

    /// <summary>
    /// @Document
    /// </summary>
    private const string DefaultLogKey = "DefaultLogger";

    
    /// <summary>
    /// @Document
    /// </summary>
    private const string LogNameKey = "LogName";

    
    /// <summary>
    /// @Document
    /// </summary>
    private const string LogLocationKey = "LogLocation";

    
    /// <summary>
    /// @Document
    /// </summary>
    private const string ApplicationNameKey = "ApplicationName";

    
    /// <summary>
    /// @Document
    /// </summary>
    private const string LogBackupCountKey = "LogBackupCount";

    #endregion

    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static ILog _logger;


    /// <summary>
    /// @Document
    /// </summary>
    private static ILog _defaultLog;


    /// <summary>
    /// @Document
    /// </summary>
    private static bool _initialized;

    #endregion

    #region Properties

    /// <summary>
    /// @Document
    /// </summary>
    public static ILog DefaultLogger => _defaultLog ?? (_defaultLog = LogManager.GetLogger(DefaultLogKey));

    #endregion

    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public static bool InitializeLogger()
    {
      if (_initialized)
        return true;
      _initialized = true;

      try
      {
        // @Document
        var disallowedCharacters = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars());
        var cleanedAppName = new string(Assembly.GetExecutingAssembly().GetName().Name
          .Where(c => !char.IsWhiteSpace(c) && !disallowedCharacters.Contains(c)).ToArray());

        // @Document
        GlobalContext.Properties[ApplicationNameKey] = cleanedAppName;
        GlobalContext.Properties[LogNameKey] = $"{cleanedAppName}_{DateTime.Today:yyyyMMdd}.log";
        GlobalContext.Properties[LogLocationKey] = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Log");
        GlobalContext.Properties[LogBackupCountKey] = 5;

        // @Document
        XmlConfigurator.Configure();

        // @Document
        _logger = ResolveLogger();
        if (_logger.IsInfoEnabled)
          _logger.InfoFormat("STARTING APPLICATION: {0} [v{1}] @ {2:g}", App.Name, App.Version, DateTime.Now.TimeOfDay);

        return true;
      }
      catch (Exception ex)
      {
        // Since it's possible that the logger is broken at this point, also write out the exception to the console
        Console.WriteLine(ex);

        try
        {
          if (DefaultLogger.IsErrorEnabled)
          DefaultLogger.Error("An exception occurred while initializing logging", ex);
        }
        catch (Exception)
        {
          // nop - If logging fails, there's not really much we can do about it
        }

        return false;
      }
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public static bool ShutdownLogger()
    {
      if (!_initialized)
        return true;
      _initialized = false;

      try
      {
        if (_logger.IsInfoEnabled)
          _logger.InfoFormat("ENDING APPLICATION: {0} [v{1}] @ {2:g}\n", App.Name, App.Version, DateTime.Now.TimeOfDay);

        LogManager.Shutdown();
        return true;
      }
      catch (Exception ex)
      {
        // Since it's possible that the logger is broken at this point, also write out the exception to the console
        Console.WriteLine(ex);

        try
        {
          if (DefaultLogger.IsErrorEnabled)
            DefaultLogger.Error("An exception occurred while shutting down logging", ex);
        }
        catch (Exception)
        {
          // nop - If logging fails, there's not really much we can do about it
        }

        return false;
      }
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ILog ResolveLogger([CallerFilePath] string sourceFilePath = null)
    {
      if (!_initialized)
        InitializeLogger();

      var callerTypeKey = Path.GetFileNameWithoutExtension(sourceFilePath);
      return string.IsNullOrWhiteSpace(callerTypeKey) ? DefaultLogger : LogManager.GetLogger(callerTypeKey);
    }

    #endregion
  }
}
