using System.Net.Mail;
using System.Runtime.InteropServices;

namespace StockingSelector.Model
{
  /// <summary>
  /// @Document
  /// </summary>
  [StructLayout(LayoutKind.Auto)]
  public struct Participant
  {
    #region Properties

    /// <summary>
    /// @Document
    /// </summary>
    public string Name { get; }


    /// <summary>
    /// @Document
    /// </summary>
    public MailAddress EmailAddress { get; }

    #endregion

    #region Ctor

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    public Participant(string name, string email)
    {
      Name = name;
      EmailAddress = new MailAddress(email);
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    public Participant(string name, MailAddress email)
    {
      Name = name;
      EmailAddress = email;
    }

    #endregion
  }
}
