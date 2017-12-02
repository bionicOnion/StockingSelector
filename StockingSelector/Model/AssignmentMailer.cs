using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using log4net;
using StockingSelector.Properties;
using StockingSelector.Utility;

namespace StockingSelector.Model
{
  /// <summary>
  /// @Document
  /// </summary>
  public class AssignmentMailer : IDisposable
  {
    #region Constants

    /// <summary>
    /// @Document
    /// </summary>
    private const string SmtpServer = "smtp.gmail.com";


    /// <summary>
    /// @Document
    /// </summary>
    private const int SmtpPort = 587;


    /// <summary>
    /// @Document
    /// </summary>
    private const uint MaxEmailAttempts = 3;


    /// <summary>
    /// @Document
    /// </summary>
    private static readonly TimeSpan SmtpConnectionTimeout = TimeSpan.FromSeconds(30);


    /// <summary>
    /// @Document
    /// </summary>
    private static readonly TimeSpan EmailReattemptTimeout = TimeSpan.FromSeconds(5);

    #endregion

    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();


    /// <summary>
    /// @Document
    /// </summary>
    private SmtpClient _smtpClient;


    /// <summary>
    /// @Document
    /// </summary>
    private MailAddress _senderAddress;


    /// <summary>
    /// @Document
    /// </summary>
    private bool _initialized;


    /// <summary>
    /// @Document
    /// </summary>
    private bool _disposed;

    #endregion

    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    public bool Initialize(MailAddress senderAddress, SecureString senderPassword)
    {
      try
      {
        if (_initialized)
        {
          if (Logger.IsWarnEnabled)
            Logger.Warn("Attempting to re-initialized an initialized object");
          return true;
        }

        // @Document
        _senderAddress = senderAddress;
        _smtpClient = new SmtpClient(SmtpServer, SmtpPort)
        {
          EnableSsl = true,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          UseDefaultCredentials = false,
          Credentials = new NetworkCredential(_senderAddress.Address, senderPassword),
          Timeout = Convert.ToInt32(SmtpConnectionTimeout.TotalMilliseconds),
        };

        _initialized = true;

        return true;
      }
      catch (Exception ex)
      {
        if (Logger.IsErrorEnabled)
          Logger.Error("An exception occurred during initialization", ex);
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
        // @Document
        if (_disposed)
        {
          if (Logger.IsWarnEnabled)
            Logger.Warn("Attempting to access a disposed object");
          return false;
        }

        // @Document
        if (!_initialized)
        {
          if (Logger.IsWarnEnabled)
            Logger.Warn("Attempting to use an uninitialized object");
          return false;
        }

        if (Logger.IsInfoEnabled)
          Logger.Info("Starting to send email notifications to participants");

        var participants = assignments.Select(a => a.Giver).ToArray();

        // @Document
        foreach (var assignment in assignments)
        {
          using (var email = new MailMessage(_senderAddress, assignment.Recipient.EmailAddress))
          {
            try
            {
              email.Subject = $"Stocking Assignment for {assignment.Giver.Name}";
              email.Body = GenerateEmailBody(assignment, participants);
              email.IsBodyHtml = true;
              
              if (Logger.IsInfoEnabled)
                Logger.InfoFormat("Sending email to participant {0}...", assignment.Giver);
              var hasSentSuccessfully = false;
              var attemptCount = 1;
              do
              {
                try
                {
                  // @Document
                  _smtpClient.Send(email);
                  hasSentSuccessfully = true;
                }
                catch (SmtpException ex)
                {
                  if (Logger.IsWarnEnabled)
                    Logger.Warn($"Caught SMTP exception with code {ex.StatusCode} for participant {assignment.Giver} on attempt {attemptCount}", ex);

                  // @Document
                  ++attemptCount;
                  Thread.Sleep(EmailReattemptTimeout);
                }
              }
              while (!hasSentSuccessfully && attemptCount <= MaxEmailAttempts);

              if (!hasSentSuccessfully && Logger.IsWarnEnabled)
                Logger.WarnFormat("Giving up on sending assignment to {0} after {1:N0} failed attempts", assignment.Giver, attemptCount - 1);
            }
            catch (Exception ex)
            {
              if (Logger.IsErrorEnabled)
                Logger.Error($"Exception thrown while sending email to participant {assignment.Giver}", ex);
            }
          }
        }

        if (Logger.IsInfoEnabled)
          Logger.Info("All emails sent");

        return true;
      }
      catch (Exception ex)
      {
        if (Logger.IsErrorEnabled)
          Logger.Error("An exception occurred during email generation/transmission", ex);
        return false;
      }
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="assignment"></param>
    /// <param name="participants"></param>
    /// <returns></returns>
    private static string GenerateEmailBody((Participant Giver, Participant Recipient) assignment,
      IReadOnlyCollection<Participant> participants)
    {
      var participantList = string.Join("\n", participants
        .Where(participant => !participant.Equals(assignment.Giver) && !participant.Equals(assignment.Recipient))
        .OrderBy(participant => participant.Name)
        .Select(p => $"<li><a href=\"{p.WishlistAddress}\">{p.Name}</a></li>"));

      return string.Format(Resources.EmailTemplate, assignment.Giver.Name, assignment.Recipient.Name,
        assignment.Recipient.WishlistAddress, participants.Count, participantList, App.Identifier);
    }

    #endregion

    #region IDisposable

    /// <summary>
    /// @Document
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }


    /// <summary>
    /// @Document
    /// </summary>
    ~AssignmentMailer()
    {
      if (Logger.IsWarnEnabled)
        Logger.WarnFormat("Finalizing an object that hasn't been properly disposed: {0}", this);
      Dispose(false);
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
      if (_disposed)
        return;
      _disposed = true;

      if (!disposing)
        return;
      
      _smtpClient?.Dispose();
    }

    #endregion
  }
}
