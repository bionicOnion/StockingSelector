using System;
using System.Windows;
using StockingSelector.Utility;

namespace StockingSelector.ViewModel
{
  /// <summary>
  /// @Document
  /// </summary>
  public abstract class BaseWindowViewModel : PropertyChangeNotifier
  {
    #region Properties

    /// <summary>
    /// @Document
    /// </summary>
    public abstract string WindowTitle { get; }


    /// <summary>
    /// @Document
    /// </summary>
    public uint WindowWidth { get; set; }


    /// <summary>
    /// @Document
    /// </summary>
    public uint WindowHeight { get; set; }


    /// <summary>
    /// @Document
    /// </summary>
    public uint WindowXPosition { get; set; }


    /// <summary>
    /// @Document
    /// </summary>
    public uint WindowYPosition { get; set; }

    #endregion
    
    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="aspectRatio"></param>
    /// <param name="screenSpaceRatio"></param>
    public void SetWindowDimensions(float aspectRatio, float screenSpaceRatio)
    {
      var targetWidth = Convert.ToUInt32(SystemParameters.PrimaryScreenWidth * screenSpaceRatio);
      var targetHeight = Convert.ToUInt32(targetWidth / aspectRatio);

      SetWindowDimensions(targetWidth, targetHeight);
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetWindowDimensions(uint width, uint height)
    {
      WindowWidth = width;
      WindowHeight = height;

      NotifyPropertiesChanged(nameof(WindowWidth), nameof(WindowHeight));
    }

    #endregion
  }
}
