using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using StockingSelector.Utility;

namespace StockingSelector.Model
{
  /// <summary>
  /// Class containing the random assignment algorithm used to pair givers with recipients
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
    /// Randomly generate an assignment mapping each participant to another participant.
    /// 
    /// No participant can be assigned their own name and the assignments themselves are determined entirely randomly
    /// </summary>
    /// <param name="participants">The collection of all participants to be included in the pairing process</param>
    /// <param name="assignments">The completed assignment mapping from givers to recipients</param>
    /// <returns>Boolean value indicating the success or failure of the assignment process</returns>
    public static bool GenerateAssignment(IReadOnlyCollection<Participant> participants,
      out IReadOnlyCollection<(Participant Giver, Participant Receiver)> assignments)
    {
      try
      {
        // Log out the number and names of all participants being included in the matching
        if (Logger.IsDebugEnabled)
          Logger.DebugFormat("Beginning pairing process with {0:N0} participants: {1}", participants.Count,
            string.Join(", ", participants.OrderBy(p => p.Name).Select(p => p.Name)));

        // Shuffle the collection of participants by ordering them according to randomly-generated numbers
        var rng = new Random();
        var shuffledParticipants = participants.OrderBy(participant => rng.Next()).ToArray();

        // Create an array of assignments by assigning each participant to the next participant in line in the shuffled
        // collection. Provided that there's more than one participant, this means that 1) nobody can be assigned their
        // own name and 2) everyone will be assigned a random name since the collection is in a random order
        var assignmentsArray = new (Participant Giver, Participant Receiver)[participants.Count];
        for (var i = 0; i < shuffledParticipants.Length; ++i)
          assignmentsArray[i] = (shuffledParticipants[i], shuffledParticipants[(i + 1) % shuffledParticipants.Length]);
        
        // Validate that the generated pairings constitute a valid assignment and enter the assignemt collection into
        // the output assignments variable
        assignments = assignmentsArray;
        return new HashSet<Participant>(assignments.Select(assn => assn.Giver)).SetEquals(participants)
            && new HashSet<Participant>(assignments.Select(assn => assn.Receiver)).SetEquals(participants);
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
