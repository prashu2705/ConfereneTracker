using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ConferenceTracker.ConferenceManagement;
using ConferenceTracker.TrackManagement;
using FluentAssertions;
using ConferenceTracker.Infrastructure;

namespace ConferenceTracker.Manager.Tests
{
    [TestFixture]
    public class ConferenceManagerTest
    {
        [TestCase(
            "Talk on ASP.NET 4.0",
            "1.5",
            false, // The title of the talk is not valid
            false)] // The length of the talk is not valid
        [TestCase(
            "Talk on ASP.NET",
            "120",
            true, // The title of the talk is valid
            false)] // The length of the talk is not valid
        [TestCase(
            "Talk on ASP.NET 4.0",
            "60",
            false, // The title of the talk is not valid
            true)] // The length of the talk is valid
        [TestCase(
            "Talk on ASP.NET",
            "60",
            true, // The title of the talk is valid
            true)] // The length of the talk is valid
        public void Schedule_a_talk(string title, string lengthOfTalk, bool isValidTitle, bool isValidDurationOfTalk)
        {
            UserInterfaceTransactionData userInput = new UserInterfaceTransactionData(title, lengthOfTalk);
            ConferenceManager.CreateScheduleFor(ref userInput);

            if (isValidTitle == false && isValidDurationOfTalk == false)
            {
                userInput.HasFaults.Should().BeTrue();
                userInput.Faults.Count.Should().Be(2);
            }
            else if ((isValidTitle && isValidDurationOfTalk == false) || (isValidTitle == false && isValidDurationOfTalk))
            {
                userInput.HasFaults.Should().BeTrue();
                userInput.Faults.Count.Should().Be(1);
            }
            else if (isValidTitle && isValidDurationOfTalk)
            {
                userInput.HasFaults.Should().BeFalse();
                ConferenceManager.Tracks.Count.Should().BeGreaterThan(0);
            }
        }

        [TestCase(true, // A null track is to be used for testing
            ExpectedException = typeof(ArgumentNullException),
            ExpectedMessage = "The track cannot be null.\r\nParameter name: track")]
        [TestCase(false)] // A non null track is to be used for testing
        public void Addition_of_track(bool isNull)
        {
            if (isNull)
            {
                Track nullTrack = null;
                ConferenceManager.AddTrack(nullTrack);
            }
            else
            {
                int numberOfExistingTracks = ConferenceManager.Tracks.Count;
                Track newTrack = new Track();
                ConferenceManager.AddTrack(newTrack);
                ConferenceManager.Tracks.Count.Should().Be(numberOfExistingTracks + 1);
            }
        }
    }
}
