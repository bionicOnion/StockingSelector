using System;
using System.Windows;
using log4net;
using StockingSelector.Utility;

namespace StockingSelector.ViewModel
{
  /// <summary>
  /// @Document
  /// </summary>
  public abstract class BaseWindowViewModel : PropertyChangeNotifier
  {
    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();

    #endregion

    #region Properties

    /// <summary>
    /// The title of the window to display on the UI
    /// </summary>
    public abstract string WindowTitle { get; }


    /// <summary>
    /// The width dimension of the window
    /// </summary>
    public uint WindowWidth { get; set; } = 800u;


    /// <summary>
    /// The height dimension of the window
    /// </summary>
    public uint WindowHeight { get; set; } = 600u;


    /// <summary>
    /// The X position of the window on the desktop
    /// </summary>
    public uint WindowXPosition { get; set; }


    /// <summary>
    /// The Y position of the window on the desktop
    /// </summary>
    public uint WindowYPosition { get; set; }

    #endregion
    
    #region Public Functions

    /// <summary>
    /// Set the dimenstions of the window as a combination of aspect ratio (width over height) and a screen space ratio
    /// (the amount of horizontal space that the window should take up on the desktop)
    /// </summary>
    /// <param name="aspectRatio">The desired aspect ratio of the window (width over height)</param>
    /// <param name="screenSpaceRatio">The desired ratio between the window's width and the size of the desktop</param>
    public void SetWindowDimensions(float aspectRatio, float screenSpaceRatio)
    {
      var targetWidth = Convert.ToUInt32(SystemParameters.PrimaryScreenWidth * screenSpaceRatio);
      var targetHeight = Convert.ToUInt32(targetWidth / aspectRatio);

      SetWindowDimensions(targetWidth, targetHeight);
    }


    /// <summary>
    /// Set the size of the window
    /// </summary>
    /// <param name="width">The desired width of the window (in pixels)</param>
    /// <param name="height">The desired height of the window (in pixels)</param>
    public void SetWindowDimensions(uint width, uint height)
    {
      WindowWidth = width;
      WindowHeight = height;
      
      if (Logger.IsInfoEnabled)
        Logger.InfoFormat("Setting dimensions of window '{0}' to {1:N0}x{2:N0}", WindowTitle, WindowWidth, WindowHeight);

      NotifyPropertiesChanged(nameof(WindowWidth), nameof(WindowHeight));
    }

    #endregion
  }
}
