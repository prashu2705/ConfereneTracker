using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ConferenceTracker.SessionManagement;
using FluentAssertions;
using ConferenceTracker.TalkManagement;

namespace ConferenceTracker.Manager.Tests
{
    /// <summary>
    /// Unit tests for class Session
    /// </summary>
    [TestFixture]
    public class SessionTest
    {
        /// <summary>
        /// The session
        /// </summary>
        private Session session;

        /// <summary>
        /// Start up method for each test.
        /// </summary>
        [SetUp]
        public void TestStartUp()
        {
            this.session = new Session(TimeOfDay.Morning);
        }

        /// <summary>
        /// Checks the total length of the session.
        /// </summary>
        [Test]
        public void Check_Total_Duration()
        {
            this.session.TotalDuration.Should().Be(0);

            this.session.AddTalk(new Talk("sample talk I", 60));
            this.session.AddTalk(new Talk("sample talk II", 30));

            this.session.TotalDuration.Should().Be(90);
        }

        /// <summary>
        /// Checks addition of talks into the session.
        /// </summary>
        /// <param name="isNull">if set to <c>true</c> The talk that is to be added into session is null; otherwise <c>false</c></param>
        [TestCase(false)]
        [TestCase(
            true, // A null talk should be used for testing
            ExpectedException = typeof(ArgumentNullException),
            ExpectedMessage = "An empty or null talk cannot be added to a session.\r\nParameter name: talk")]
        public void Addition_of_talks(bool isNull)
        {
            if (isNull)
            {
                Talk nullTalk = null;
                try
                {
                    this.session.AddTalk(nullTalk);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                this.session.AddTalk(new Talk("sample talk I", 60));
                this.session.AddTalk(new Talk("sample talk II", 30));
                this.session.Talks.Count.Should().Be(2);
            }
        }

        /// <summary>
        /// Checks the total permissible length of the session.
        /// </summary>
        /// <param name="sessionType">Type of the session.</param>
        /// <param name="totalPossibleDuration">Total duration of the possible.</param>
        [TestCase(
            TimeOfDay.Morning, // The test is to be on a morning session
            180, // Permissible limit on duration for a morning session
            ExpectedException = typeof(InvalidOperationException), 
            ExpectedMessage = "There are no slots left to accommodate the provided talk.")]
        [TestCase(
            TimeOfDay.Afternoon, // The test is to be on an afternoon session
            240, // Permissible limit on duration for an afternoon session
            ExpectedException = typeof(InvalidOperationException),
            ExpectedMessage = "There are no slots left to accommodate the provided talk.")]
        public void Check_threshold_of_total_permissible_length_of_Session(TimeOfDay sessionType, int totalPossibleDuration)
        {
            this.session = new Session(sessionType);
            int sumOfTheLengthOfTalksAdded = 0;

            try
            {
                this.session.AddTalk(new Talk("sample talk I", 60));
                sumOfTheLengthOfTalksAdded += 60;
                this.session.AddTalk(new Talk("sample talk II", 45));
                sumOfTheLengthOfTalksAdded += 45;
                this.session.AddTalk(new Talk("sample talk III", 60));
                sumOfTheLengthOfTalksAdded += 60;
                this.session.AddTalk(new Talk("sample talk IV", 45));
                sumOfTheLengthOfTalksAdded += 45;
                this.session.AddTalk(new Talk("sample talk IV", 45));
                sumOfTheLengthOfTalksAdded += 45;
            }
            catch (Exception)
            {
                this.session.TotalDuration.Should().Be(sumOfTheLengthOfTalksAdded);
                throw;
            }
        }

        /// <summary>
        /// Checks removal of talks from the session
        /// </summary>
        [Test]
        public void Removal_of_talks()
        {
            this.session.AddTalk(new Talk("sample talk I", 60));
            this.session.RemoveTalk("sample talk I");
            this.session.Talks.Count.Should().Be(0);

            this.session.AddTalk(new Talk("sample talk I", 60));
            this.session.AddTalk(new Talk("sample talk II", 60));
            this.session.RemoveTalk("sample talk I");
            this.session.Talks.Count.Should().Be(1);

            Talk talk = this.session.Talks.First();
            talk.Should().NotBeNull();
            talk.Title.Should().Be("sample talk II");
            
        }
    }
}
