using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using log4net;
using StockingSelector.Properties;
using StockingSelector.Utility;
using SpecialFolder = System.Environment.SpecialFolder;

namespace StockingSelector.Model
{
  /// <summary>
  /// @Document
  /// </summary>
  public class AssignmentMailer
  {
    #region Constants

    /// <summary>
    /// @Document
    /// </summary>
    private const string CredentialsLocation = ".credentials/gmail-stocking-selector-credentials.json";


    /// <summary>
    /// @Document
    /// </summary>
    private static readonly string[] GmailScopes = { GmailService.Scope.GmailCompose, GmailService.Scope.GmailSend };

    #endregion

    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();

    /// <summary>
    /// @Document
    /// </summary>
    private GmailService _gmailService;

    #endregion

    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Authenticate()
    {
      try
      {
        // @Log

        // @Document
        ClientSecrets clientSecrets;
        using (var stream = new MemoryStream(Resources.gmail_client_secret))
          clientSecrets = GoogleClientSecrets.Load(stream).Secrets;
        var credentialsPath = Path.Combine(Environment.GetFolderPath(SpecialFolder.Personal), CredentialsLocation);
        var credentialsStore = new FileDataStore(credentialsPath, true);

        // @Unimplemented Figure out the user name?
        var authorizationToken = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets,
          GmailScopes, string.Empty, CancellationToken.None, credentialsStore);

        // @Log

        // @Document
        await authorizationToken;

        // @Log

        // @Document
        _gmailService = new GmailService(new BaseClientService.Initializer
        {
          HttpClientInitializer = authorizationToken.Result,
          ApplicationName = App.Name,
        });

        // @Log

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex); // @Log
        return false;
      }
    }

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="assignments"></param>
    /// <returns></returns>
    public bool MailAssignments(IReadOnlyCollection<(Participant Giver, Participant Recipient)> assignments)
    {
      try
      {
        // @Log

        // @Document (and also log and/or send a status message)
        if (_gmailService == null)
          return false;

        // @Unimplemented Compose and send all of the emails

        // @Log

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex); //@Log
        return false;
      }
    }

    #endregion
  }
}
