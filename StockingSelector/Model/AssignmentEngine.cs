using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using StockingSelector.Utility;

namespace StockingSelector.Model
{
  /// <summary>
  /// @Document
  /// </summary>
  public static class AssignmentEngine
  {
    #region Fields

    /// <summary>
    /// The logger used for events within this class
    /// </summary>
    private static readonly ILog Logger = LoggingUtilities.ResolveLogger();

    #endregion

    #region Public Functions

    /// <summary>
    /// @Document
    /// </summary>
    /// <param name="participants"></param>
    /// <param name="assignments"></param>
    /// <returns></returns>
    public static bool GenerateAssignment(IReadOnlyCollection<Participant> participants,
      out IReadOnlyCollection<(Participant Giver, Participant Recipient)> assignments)
    {
      try
      {
        // @Log

        // @Document
        var rng = new Random();
        var shuffledParticipants = participants.OrderBy(participant => rng.Next()).ToArray();

        // @Document
        var assignmentsArray = new (Participant Giver, Participant Recipient)[participants.Count];
        for (var i = 0; i < shuffledParticipants.Length; ++i)
          assignmentsArray[i] = (shuffledParticipants[i], shuffledParticipants[(i + 1) % shuffledParticipants.Length]);

        // @Log

        // @Document
        assignments = assignmentsArray;
        return new HashSet<Participant>(assignmentsArray.Select(assignment => assignment.Giver)).SetEquals(participants)
            && new HashSet<Participant>(assignmentsArray.Select(assignment => assignment.Recipient)).SetEquals(participants);
      }
      catch (Exception ex)
      {
        if (Logger.IsErrorEnabled)
          Logger.Error("Exception occurred during assignment generation", ex);

        assignments = null;
        return false;
      }
    }

    #endregion
  }
}
