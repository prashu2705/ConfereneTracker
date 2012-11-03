using System.Linq;
using ConferenceTracker.SessionManagement;
using ConferenceTracker.TrackManagement;
using NUnit.Framework;
using System;
using FluentAssertions;

namespace ConferenceTracker.Manager.Tests
{
    /// <summary>
    /// Unit tests for class Talk
    /// </summary>
    [TestFixture]
    public class TrackTest
    {
        /// <summary>
        /// The track
        /// </summary>
        private Track track;

        /// <summary>
        /// Start up task before each test.
        /// </summary>
        [SetUp]
        public void TestStartUp()
        {
            this.track = new Track();
        }

        /// <summary>
        /// Checks the instantiation of class track.
        /// </summary>
        [Test]
        public void Instantiation_of_track()
        {
            this.track.Sessions.Should().NotBeNull();
            this.track.Sessions.Count.Should().Be(2);
            this.track.Sessions.Any(session => session.TimeOfDay == TimeOfDay.Morning).Should().BeTrue();
            this.track.Sessions.Any(session => session.TimeOfDay == TimeOfDay.Afternoon).Should().BeTrue();
        }

        /// <summary>
        /// Checks the addition of sessions into the track
        /// </summary>
        /// <param name="isNull">if set to <c>true</c> indicates that that the session to be used for test is null;
        /// otherwise <c>false</c>.</param>
        /// <param name="isExisting">if set to <c>true</c> indicates that the session to be used for test is already existing;
        /// otherwise <c>false</c>.</param>
        [TestCase(
            true, // Session to be used for test is null
            false, // Session to be used for test is not already existing in the track
            ExpectedException = typeof(ArgumentNullException),
            ExpectedMessage = "An empty or null session cannot be added to a track.\r\nParameter name: session")]
        [TestCase(
            false, // Session to be used for test is not null
            true, // Session to be used for test is already existing in the track
            ExpectedException = typeof(ArgumentException),
            ExpectedMessage = "The provided session cannot be added to this track, as a session for the time of the day already exists.")]
        [TestCase(
            false, // Session to be used for test is not null
            false)] // Session to be used for test is not already existing in the track
        public void Addition_of_session(bool isNull, bool isExisting)
        {
            if (isNull)
            {
                Session nullSession = null;
                this.track.AddSession(nullSession);
            }
            else
            {
                if (isExisting)
                {
                    this.track.AddSession(new Session(TimeOfDay.Morning));
                }
                else
                {
                    this.track.RemoveSession(TimeOfDay.Morning);
                    this.track.AddSession(new Session(TimeOfDay.Morning));
                }
            }
        }

        /// <summary>
        /// Checks removal of sessions from the track
        /// </summary>
        /// <param name="timeOfDay">The time of day.</param>
        [TestCase(TimeOfDay.Afternoon)]
        [TestCase(TimeOfDay.Morning)]
        public void Removal_of_session(TimeOfDay timeOfDay)
        {
            this.track.RemoveSession(timeOfDay);

            this.track.Sessions.Count.Should().Be(1);
            this.track.Sessions.Any(session => session.TimeOfDay == timeOfDay).Should().BeFalse();
        }
    }
}
