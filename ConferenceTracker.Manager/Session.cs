using System;
using System.Collections.Generic;
using ConferenceTracker.TalkManagement;
using System.Collections.ObjectModel;

namespace ConferenceTracker.SessionManagement
{
    /// <summary>
    /// A class that represents a Session
    /// </summary>
    public class Session
    {
        /// <summary>
        /// The total possible duration
        /// </summary>
        private int totalPossibleDuration;

        /// <summary>
        /// The private instance field for list of talks
        /// </summary>
        private List<Talk> _talks;

        /// <summary>
        /// Gets the talks.
        /// </summary>
        /// <value>The talks.</value>
        public ReadOnlyCollection<Talk> Talks
        {
            get
            {
                return _talks.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the time of day.
        /// </summary>
        /// <value>The time of day.</value>
        private readonly TimeOfDay _timeOfDay;

        /// <summary>
        /// Gets the time of day.
        /// </summary>
        /// <value>The time of day.</value>
        public TimeOfDay TimeOfDay
        {
            get
            {
                return _timeOfDay;
            }
        }

        /// <summary>
        /// Gets the total duration.
        /// </summary>
        /// <value>The total duration.</value>
        public int TotalDuration
        {
            get
            {
                if (this._talks != null && this._talks.Count > 0)
                {
                    // Get the summation of the duration of all the scheduled talks
                    int duration = 0;
                    this._talks.ForEach((talk) =>
                    {
                        duration += talk.Duration;
                    });

                    // Return the total duration
                    return duration;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session" /> class.
        /// </summary>
        /// <param name="timeOfDay">The time of day.</param>
        public Session(TimeOfDay timeOfDay)
        {
            this._talks = new List<Talk>();
            this._timeOfDay = timeOfDay;

            // Total possible duration can be 3 hrs if it is a morning session; 4 hrs otherwise
            this.totalPossibleDuration = timeOfDay == TimeOfDay.Morning ? 60 * 3 : 60 * 4;
        }

        /// <summary>
        /// Adds the talk.
        /// </summary>
        /// <param name="talk">The talk.</param>
        /// <exception cref="System.ArgumentNullException">talk;An empty or null talk cannot be added to a session.</exception>
        /// <exception cref="System.InvalidOperationException">There are no slots left to accommodate the provided talk.</exception>
        public void AddTalk(Talk talk)
        {
            if (talk == null)
            {
                throw new ArgumentNullException("talk", "An empty or null talk cannot be added to a session.");
            }

            // Is the session possible to accommodate the specified talk?
            if (!this.CanAdd_a_Talk(talk))
            {
                throw new InvalidOperationException("There are no slots left to accommodate the provided talk.");
            }

            this._talks.Add(talk);
        }

        /// <summary>
        /// Removes the talk.
        /// </summary>
        /// <param name="title">The title.</param>
        public void RemoveTalk(string title)
        {
            Talk talkToBeRemoved = this._talks.Find(talk => string.Equals(talk.Title, title, StringComparison.InvariantCultureIgnoreCase));
            this._talks.Remove(talkToBeRemoved);
        }

        /// <summary>
        /// Determines whether this instance can accommodate the specified talk.
        /// </summary>
        /// <param name="talk">The talk.</param>
        /// <returns><c>true</c> if this instance can accommodate the specified talk; otherwise, <c>false</c>.</returns>
        public bool CanAccommodateThis(Talk talk)
        {
            return this.CanAdd_a_Talk(talk);
        }

        /// <summary>
        /// Determines whether this instance can add the specified talk.
        /// </summary>
        /// <param name="talk">The talk.</param>
        /// <returns><c>true</c> if this instance can add the specified talk; otherwise, <c>false</c>.</returns>
        private bool CanAdd_a_Talk(Talk talk)
        {
            // Does the total duration after including the provided talk exceed the permissible length of the session
            if (this.TotalDuration + talk.Duration > this.totalPossibleDuration)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Enum TimeOfDay
    /// </summary>
    public enum TimeOfDay
    {
        /// <summary>
        /// The morning
        /// </summary>
        Morning,
        /// <summary>
        /// The afternoon
        /// </summary>
        Afternoon
    }
}