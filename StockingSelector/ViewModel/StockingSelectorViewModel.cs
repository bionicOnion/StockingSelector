namespace StockingSelector.ViewModel
{
  /// <summary>
  /// @Document
  /// </summary>
  internal class StockingSelectorViewModel : BaseWindowViewModel
  {
    #region Properties

    /// <summary>
    /// @Comment
    /// </summary>
    public override string WindowTitle => App.Name;

    #endregion

    #region Ctor

    public StockingSelectorViewModel()
    {
      // @Improvement Pass these as parameters, load window state from WindowPositionSettings
      SetWindowDimensions(1.3333f, 0.3333f);
    }

    #endregion
  }
}
