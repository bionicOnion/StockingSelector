using System;
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


    /// <summary>
    /// @Document
    /// </summary>
    public Uri WishlistAddress { get; }

    #endregion

    #region Ctor

    /// <summary>
    /// Create a new participant with the given name and email address
    /// </summary>
    /// <param name="name">The name of the participant</param>
    /// <param name="email">An email address at which the participant can be reached</param>
    /// <param name="wishlistAddress">@Document</param>
    public Participant(string name, string email, string wishlistAddress)
    {
      Name = name;
      EmailAddress = new MailAddress(email);
      WishlistAddress = new Uri(wishlistAddress, UriKind.RelativeOrAbsolute);
    }


    /// <summary>
    /// Create a new participant with the given name and email address
    /// </summary>
    /// <param name="name">The name of the participant</param>
    /// <param name="email">An email address at which the participant can be reached</param>
    /// <param name="wishlistAddress">@Document</param>
    public Participant(string name, MailAddress email, Uri wishlistAddress)
    {
      Name = name;
      EmailAddress = email;
      WishlistAddress = wishlistAddress;
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      var other = obj as Participant?;
      return other.HasValue
          && other.Value.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)
          && other.Value.EmailAddress.Equals(EmailAddress)
          && other.Value.WishlistAddress.Equals(WishlistAddress);
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Name?.GetHashCode() ?? 0;


    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Name} ({EmailAddress}; {WishlistAddress})";

    #endregion
  }
}
