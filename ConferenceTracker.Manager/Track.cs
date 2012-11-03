using System;
using System.Collections.Generic;
using ConferenceTracker.SessionManagement;
using System.Collections.ObjectModel;

namespace ConferenceTracker.TrackManagement
{
    /// <summary>
    /// A class that represents a Track
    /// </summary>
    public class Track
    {
        /// <summary>
        /// The private instance field of list of sessions
        /// </summary>
        private List<Session> _sessions;

        /// <summary>
        /// Gets the sessions.
        /// </summary>
        /// <value>The sessions.</value>
        public ReadOnlyCollection<Session> Sessions
        {
            get
            {
                return _sessions.AsReadOnly();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track" /> class.
        /// </summary>
        public Track()
        {
            this._sessions = new List<Session>() { new Session(TimeOfDay.Morning), new Session(TimeOfDay.Afternoon) };
        }

        /// <summary>
        /// Adds the session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <exception cref="System.ArgumentNullException">session;An empty or null session cannot be added to a track.</exception>
        /// <exception cref="System.InvalidOperationException">The provided session cannot be added to this track, as a session for the time of the day already exists.</exception>
        public void AddSession(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session", "An empty or null session cannot be added to a track.");
            }

            if (!this.CanAdd_a_Session(session))
            {
                throw new ArgumentException("The provided session cannot be added to this track, as a session for the time of the day already exists.");
            }

            this._sessions.Add(session);
        }

        /// <summary>
        /// Removes the session.
        /// </summary>
        /// <param name="timeOfDay">The time of day.</param>
        /// <exception cref="System.ArgumentNullException">session;It is impossible to remove this session as it is empty.</exception>
        public void RemoveSession(TimeOfDay timeOfDay)
        {
            Session sessionToBeRemoved = this._sessions.Find(session => session.TimeOfDay == timeOfDay);
            this._sessions.Remove(sessionToBeRemoved);
        }

        /// <summary>
        /// Determines whether this instance can accommodate the specified session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns><c>true</c> if this instance [can accommodate the specified session; otherwise, <c>false</c>.</returns>
        public bool CanAccommodateThis(Session session)
        {
            return this.CanAdd_a_Session(session);
        }

        /// <summary>
        /// Determines whether this instance [can add_a_ session] the specified session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns><c>true</c> if this instance [can add_a_ session] the specified session; otherwise, <c>false</c>.</returns>
        private bool CanAdd_a_Session(Session session)
        {
            if (this._sessions.Count < 1)
            {
                return true;
            }

            if (this._sessions.Count > 1)
            {
                return false;
            }

            return !(this._sessions.Exists(existingSession => existingSession.TimeOfDay == session.TimeOfDay));
        }
    }
}
