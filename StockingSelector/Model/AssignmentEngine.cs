using System;
using System.Collections.Generic;
using System.Linq;

namespace StockingSelector.Model
{
  /// <summary>
  /// @Document
  /// </summary>
  public static class AssignmentEngine
  {
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
        // @Document
        var rng = new Random();
        var shuffledParticipants = participants.OrderBy(participant => rng.Next()).ToArray();

        // @Document
        var assignmentsArray = new (Participant Giver, Participant Recipient)[participants.Count];
        for (var i = 0; i < shuffledParticipants.Length; ++i)
          assignmentsArray[i] = (shuffledParticipants[i], shuffledParticipants[(i + 1) % shuffledParticipants.Length]);

        // @Document
        assignments = assignmentsArray;
        return new HashSet<Participant>(assignmentsArray.Select(assignment => assignment.Giver)).SetEquals(participants)
            && new HashSet<Participant>(assignmentsArray.Select(assignment => assignment.Recipient)).SetEquals(participants);
      }
      catch (Exception ex)
      {
        // @Unimplemented Add logging here
        Console.WriteLine(ex);

        assignments = null;
        return false;
      }
    }

    #endregion
  }
}
