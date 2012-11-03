using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ConferenceTracker.Infrastructure;
using ConferenceTracker.SessionManagement;
using ConferenceTracker.TalkManagement;
using ConferenceTracker.TrackManagement;

namespace ConferenceTracker.ConferenceManagement
{
    /// <summary>
    /// A class that manages the conference
    /// </summary>
    public sealed class ConferenceManager
    {
        /// <summary>
        /// The list of tracks
        /// </summary>
        private static List<Track> _tracks;

        /// <summary>
        /// Gets the tracks.
        /// </summary>
        /// <value>The tracks.</value>
        public static ReadOnlyCollection<Track> Tracks
        {
            get
            {
                if (_tracks == null)
                {
                    _tracks = new List<Track>();
                }
                return _tracks.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void AddTrack(Track track)
        {
            if (track == null)
            {
                throw new ArgumentNullException("track", "The track cannot be null.");
            }

            _tracks = GetListOfTracks();
            _tracks.Add(track);
        }

        /// <summary>
        /// Creates the schedule for the provided user input.
        /// </summary>
        /// <param name="uiTransactionData">The user input.</param>
        public static void CreateScheduleFor(ref UserInterfaceTransactionData uiTransactionData)
        {
            try
            {
                // Validate the user provided input
                ValidateUserInput(ref uiTransactionData);

                // Check whether the user input has any faults and proceed accordingly
                if (uiTransactionData.HasFaults == false)
                {
                    // Get the length of the talk in expected data type
                    int durationOfTalk = 0;
                    int.TryParse(uiTransactionData.LengthOfTalk, out durationOfTalk);

                    // Create a new talk
                    Talk newTalk = new Talk(uiTransactionData.TalkTitle, durationOfTalk);

                    // Add the talk to one of the available tracks
                    if (AddTalkToTrack(newTalk) == false)
                    {
                        // If the talk could not be added, induce an error into the user input
                        uiTransactionData.Faults.Add(new Fault() { Message = "Talk could not be scheduled.", Code = "Talk" });
                    }
                }
            }
            catch (Exception exception)
            {
                uiTransactionData.Faults.Add(new Fault() { Message = exception.Message, Code = exception.Source });
            }
        }

        /// <summary>
        /// Validates the user input.
        /// </summary>
        /// <param name="uiTransactionData">The user input.</param>
        public static void ValidateUserInput(ref UserInterfaceTransactionData uiTransactionData)
        {
            // Check whether the title of the talk input by the user is valid,
            // otherwise add the error into the user input instance
            if (!Talk.IsValidTitle(uiTransactionData.TalkTitle))
            {
                Fault newError = new Fault() { Message = "The title of the talk should not contain any numbers", Code = "userInput.TalkTitle" };
                uiTransactionData.Faults.Add(newError);
            }

            // Check whether the length of the talk input by the user is valid,
            // otherwise add the error into the user input instance
            int result = 0;
            if (!Regex.IsMatch(uiTransactionData.LengthOfTalk, @"^\d+$") && !int.TryParse(uiTransactionData.LengthOfTalk, out result))
            {
                Fault newError = new Fault() { Message = "The duration of the talk should be numbers only", Code = "userInput.LengthOfTalk" };
                uiTransactionData.Faults.Add(newError);
            }
        }

        /// <summary>
        /// Adds the talk to a track.
        /// </summary>
        /// <param name="talk">The talk.</param>
        /// <returns><c>true</c> if the given talk was added into a session of a track, <c>false</c> otherwise</returns>
        private static bool AddTalkToTrack(Talk talk)
        {
            List<Track> _listOfTracks = GetListOfTracks();

            // Whether the talk has already been accommodated into a session
            bool talkHasBeenAdded = false;

            // Check whether any of the tracks has a session with a slot left for the talk
            foreach (Track track in _listOfTracks)
            {
                // Check whether the track already has any sessions
                if (track.Sessions.Count == 0)
                {
                    // Create a new session and add the talk into the session 
                    Session newSession = new Session(TimeOfDay.Morning);
                    track.AddSession(newSession);
                }

                // Check whether the given talk can be accommodated in one of the sessions of the given track
                AddTalkToSession(track, talk, out talkHasBeenAdded);

                // If the talk has been accommodated in a session, 
                // there is no need to check for slots in other tracks
                if (talkHasBeenAdded)
                {
                    break;
                }
            }

            // If track has not been added yet, a new track has to be created
            if (talkHasBeenAdded == false)
            {
                // Add the given talk to one of the sessions of the new track
                Track newTrack = AddTalkToSession(new Track(), talk, out talkHasBeenAdded);

                // Update the list of tracks with the new track
                _tracks.Add(newTrack);
            }

            return talkHasBeenAdded;
        }

        /// <summary>
        /// Adds the talk to a session.
        /// </summary>
        /// <param name="aTrack">A track.</param>
        /// <param name="talk">The talk.</param>
        /// <param name="talkHasBeenAdded">if set to <c>true</c> [talk has been added].</param>
        /// <returns>Track.</returns>
        private static Track AddTalkToSession(Track aTrack, Talk talk, out bool talkHasBeenAdded)
        {
            // The given talk has not been added yet
            talkHasBeenAdded = false;

            // Check and add the talk to the session in the track
            foreach (Session aSession in aTrack.Sessions)
            {
                // Whether the session can accommodate the talk
                if (aSession.CanAccommodateThis(talk))
                {
                    // Add the talk into the session
                    aSession.AddTalk(talk);
                    talkHasBeenAdded = true;
                    break;
                }
            }

            return aTrack;
        }

        /// <summary>
        /// Gets the list of tracks.
        /// </summary>
        /// <returns>List{Track}.</returns>
        private static List<Track> GetListOfTracks()
        {
            if (_tracks == null)
            {
                _tracks = new List<Track>();
            }

            return _tracks;
        }
    }
}
