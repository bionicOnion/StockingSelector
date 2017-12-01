using System.Net.Mail;
using System.Runtime.InteropServices;

namespace StockingSelector.Model
{
  /// <summary>
  /// Struct containing information about a participant included in the stocking matching
  /// </summary>
  [StructLayout(LayoutKind.Auto)]
  public struct Participant
  {
    #region Properties

    /// <summary>
    /// The name of the participant
    /// </summary>
    public string Name { get; }


    /// <summary>
    /// The email address to send information about a participant's assignment to
    /// </summary>
    public MailAddress EmailAddress { get; }

    #endregion

    #region Ctor

    /// <summary>
    /// Create a new participant with the given name and email address
    /// </summary>
    /// <param name="name">The name of the participant</param>
    /// <param name="email">An email address at which the participant can be reached</param>
    public Participant(string name, string email)
    {
      Name = name;
      EmailAddress = new MailAddress(email);
    }


    /// <summary>
    /// Create a new participant with the given name and email address
    /// </summary>
    /// <param name="name">The name of the participant</param>
    /// <param name="email">An email address at which the participant can be reached</param>
    public Participant(string name, MailAddress email)
    {
      Name = name;
      EmailAddress = email;
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Name} ({EmailAddress})";

    #endregion
  }
}
