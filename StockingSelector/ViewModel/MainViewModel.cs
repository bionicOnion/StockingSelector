using log4net;
using StockingSelector.Utility;

namespace StockingSelector.ViewModel
{
  /// <summary>
  /// @Document
  /// </summary>
  internal class MainViewModel : BaseWindowViewModel
  {
    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();

    #endregion

    #region Properties

    /// <summary>
    /// @Comment
    /// </summary>
    public override string WindowTitle { get; } = $"{App.Name} [v{App.Version}]";

    #endregion

    #region Ctor

    public MainViewModel()
    {
      // @Improvement Pass these as parameters, load window state from WindowPositionSettings
      SetWindowDimensions(1.3333f, 0.3333f);
    }

    #endregion
  }
}
