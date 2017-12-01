using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Security;
using System.Windows.Input;
using log4net;
using StockingSelector.Model;
using StockingSelector.Utility;
using StockingSelector.View.Command;

namespace StockingSelector.ViewModel
{
  /// <summary>
  /// @Document
  /// </summary>
  internal class MainViewModel : BaseWindowViewModel, IDisposable
  {
    #region Fields

    /// <summary>
    /// @Document
    /// </summary>
    private readonly AssignmentMailer _mailer;


    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();


    /// <summary>
    /// @Document
    /// </summary>
    private bool _disposed;


    private ICommand _generateAssignmentCommand;
    private ICommand _addParticipantCommand;
    private ICommand _removeParticipantCommand;

    #endregion

    #region Properties

    /// <summary>
    /// @Comment
    /// </summary>
    public override string WindowTitle { get; } = $"{App.Name} [v{App.Version}]";


    /// <summary>
    /// Collection of all participants slated to take part in the stocking exchange
    /// </summary>
    public ObservableCollection<Participant> Participants { get; } = new ObservableCollection<Participant>();


    /// <summary>
    /// @Document
    /// </summary>
    public string NewParticipantName { get; set; }


    /// <summary>
    /// @Document
    /// </summary>
    public string NewParticipantEmailAddress { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// @Document
    /// </summary>
    public ICommand GenerateAssignmentsCommand => _generateAssignmentCommand ??
      (_generateAssignmentCommand = new DelegateCommand(OnGenerateAssignmentCommand));

    /// <summary>
    /// @Document
    /// </summary>
    public ICommand AddParticipantCommand => _addParticipantCommand ??
      (_addParticipantCommand = new DelegateCommand(OnAddParticipantCommand));

    /// <summary>
    /// @Document
    /// </summary>
    public ICommand RemoveParticipantCommand => _removeParticipantCommand ??
      (_removeParticipantCommand = new DelegateCommand(OnRemoveParticipantCommand));

    #endregion

    #region Ctor

    /// <summary>
    /// @Document
    /// </summary>
    public MainViewModel()
    {
      // @Improvement Pass these as parameters, load window state from WindowPositionSettings
      SetWindowDimensions(1.3333f, 0.3333f);

      _mailer = new AssignmentMailer();
    }

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
        return _mailer.Initialize(senderAddress, senderPassword);
      }
      catch (Exception ex)
      {
        if (Logger.IsErrorEnabled)
          Logger.Error("An exception occurred during initialization", ex);
        return false;
      }
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// @Document
    /// </summary>
    private void OnGenerateAssignmentCommand()
    {
      // @Document
      if (!AssignmentEngine.GenerateAssignment(Participants, out var assignments))
      {
        if (Logger.IsWarnEnabled)
          Logger.Warn("Failed to generate an assignment for the provided participants");
        return;
      }

      // @Document
      if (!_mailer.MailAssignments(assignments))
      {
        if (Logger.IsWarnEnabled)
          Logger.Warn("Failed to mail assignments out to participants");
      }
    }

    /// <summary>
    /// @Document
    /// </summary>
    private void OnAddParticipantCommand()
    {
      // If the name or email address hasn't been specified, there's nothing to do
      if (string.IsNullOrWhiteSpace(NewParticipantName) || string.IsNullOrWhiteSpace(NewParticipantEmailAddress))
      {
        if (Logger.IsDebugEnabled)
          Logger.DebugFormat("Skipping addition of participant with missing data: Name={0}; Email={1}",
            NewParticipantName == string.Empty ? "<EMPTY>" : NewParticipantName ?? "<NULL>",
            NewParticipantEmailAddress == string.Empty ? "<EMPTY>" : NewParticipantEmailAddress ?? "<NULL>");
        return;
      }

      // @Document
      var participant = new Participant(NewParticipantName, NewParticipantEmailAddress);
      Participants.Add(participant);
      if (Logger.IsDebugEnabled)
        Logger.DebugFormat("Adding participant {0} (email address {1}) to collection; {2:N0} participants configured",
          participant.Name, participant.EmailAddress, Participants.Count);

      // @Document
      NewParticipantName = string.Empty;
      NewParticipantEmailAddress = string.Empty;
      NotifyPropertiesChanged(nameof(NewParticipantName), nameof(NewParticipantEmailAddress));
    }


    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="param"></param>
    private void OnRemoveParticipantCommand(object param)
    {
      if (!(param is Participant participant))
      {
        if (Logger.IsWarnEnabled)
          Logger.WarnFormat("Parameter was not of expected type {0}; was {1}", nameof(Participant),
            param.GetType().Name);
      }
      else if (!Participants.Remove(participant))
      {
        if (Logger.IsWarnEnabled)
          Logger.WarnFormat("Failed to remove participant {0} from collection", participant);
      }
      else
      {
        // @Unimplemented Notify about the removal
      }
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
    ~MainViewModel()
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

      _mailer.Dispose();
    }

    #endregion
  }
}
