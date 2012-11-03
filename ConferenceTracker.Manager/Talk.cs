using System;
using System.Text.RegularExpressions;

namespace ConferenceTracker.TalkManagement
{
    /// <summary>
    /// A class that represents a Talk
    /// </summary>
    public class Talk
    {
        /// <summary>
        /// The topic of the talk
        /// </summary>
        /// <value>The title.</value>
        private readonly string _title;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _title;
            }
        }

        /// <summary>
        /// The duration of the talk in minutes
        /// </summary>
        /// <value>The duration.</value>
        private readonly int _duration;

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public int Duration
        {
            get
            {
                return _duration;
            }
        }

        /// <summary>
        /// The start time of the talk
        /// </summary>
        /// <value>The start time.</value>
        private readonly DateTime _startTime;

        /// <summary>
        /// Gets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Talk" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="durationInMinutes">The duration in minutes.</param>
        /// <exception cref="System.ArgumentException">The length of the talk cannot be more than an hour or less than 5 minutes.;startTime</exception>
        public Talk(string title, int durationInMinutes)
        {
            if (Talk.IsTooLong(durationInMinutes) || Talk.IsTooShort(durationInMinutes))
            {
                throw new ArgumentException("The length of the talk cannot be more than an hour or less than 5 minutes.", "duration");
            }

            if (!Talk.IsValidTitle(title))
            {
                throw new ArgumentException("The title of the talk is not valid.", "title");
            }

            this._title = title;
            this._duration = durationInMinutes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Talk" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="durationInMinutes">The duration in minutes.</param>
        /// <param name="startTime">The start time.</param>
        /// <exception cref="System.ArgumentException">The start time specified cannot be earlier than 9:00 hrs.;startTime</exception>
        public Talk(string title, int durationInMinutes, DateTime startTime)
            : this(title, durationInMinutes)
        {
            if (Talk.StartsVeryEarly(startTime.TimeOfDay))
            {
                throw new ArgumentException("The start time specified cannot be earlier than 9:00 hrs.", "startTime");
            }

            this._startTime = startTime;
        }

        /// <summary>
        /// Determines whether the specified title is valid.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns><c>true</c> if the specified title is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidTitle(string title)
        {
            return !(Regex.IsMatch(title, @"\d+")) && (Regex.IsMatch(title, @"^[A-Za-z]+"));
        }

        /// <summary>
        /// Determines whether the specified duration in minutes is too short.
        /// </summary>
        /// <param name="durationInMinutes">The duration in minutes.</param>
        /// <returns><c>true</c> if [is too short] [the specified duration in minutes]; otherwise, <c>false</c>.</returns>
        private static bool IsTooShort(int durationInMinutes)
        {
            return (durationInMinutes < 5);
        }

        /// <summary>
        /// Determines whether the specified duration in minutes is too long.
        /// </summary>
        /// <param name="durationInMinutes">The duration in minutes.</param>
        /// <returns><c>true</c> if [is too long] [the specified duration in minutes]; otherwise, <c>false</c>.</returns>
        private static bool IsTooLong(int durationInMinutes)
        {
            return (durationInMinutes > 60);
        }

        /// <summary>
        /// Determines whether the specified time is very early.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private static bool StartsVeryEarly(TimeSpan time)
        {
            return (TimeSpan.FromHours(time.TotalMilliseconds).CompareTo(new TimeSpan(9, 0, 0)) < 0);
        }
    }
}
