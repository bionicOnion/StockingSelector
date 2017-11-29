using System;
using System.Runtime.CompilerServices;

namespace StockingSelector.Utility
{
  /// <summary>
  /// @Document
  /// </summary>
  public enum ResultCode
  {
    /// <summary>
    /// @Document
    /// </summary>
    Success = 0,

    /// <summary>
    /// @Document
    /// </summary>
    LifecycleFailure = -1,

    /// <summary>
    /// @Document
    /// </summary>
    UnhandledDispatcherException = -2,

    /// <summary>
    /// @Document
    /// </summary>
    LoggerFailure = -3,
  }


  /// <summary>
  /// @Document
  /// </summary>
  public static class ResultCodeExtensions
  {
    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this ResultCode code) => Convert.ToInt32(code);


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsError(this ResultCode code) => code != ResultCode.Success;
  }
}
