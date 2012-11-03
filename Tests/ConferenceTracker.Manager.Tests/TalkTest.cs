using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ConferenceTracker.TalkManagement;
using NUnit.Framework;
using FluentAssertions;

namespace ConferenceTracker.Manager.Tests
{
    /// <summary>
    /// Unit tests for class Talk
    /// </summary>
    [TestFixture]
    public class TalkTest
    {
        /// <summary>
        /// Checks the correctness of the talk details
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="lengthOfTalk">The length of talk.</param>
        [TestCase(
            "test talk title 145",
            30,
            ExpectedException = typeof(ArgumentException),
            ExpectedMessage = "The title of the talk is not valid.\r\nParameter name: title")]
        [TestCase(
            "Talk on C++ and C#",
            60)]
        [TestCase(
            "Some title for the talk.",
            90,
            ExpectedException = typeof(ArgumentException),
            ExpectedMessage = "The length of the talk cannot be more than an hour or less than 5 minutes.\r\nParameter name: duration")]
        [TestCase(
            "Some title for the talk.",
            1,
           ExpectedException = typeof(ArgumentException),
           ExpectedMessage = "The length of the talk cannot be more than an hour or less than 5 minutes.\r\nParameter name: duration")]
        public void Validation_of_details_of_talk(string title, int lengthOfTalk)
        {
            Talk talk = new Talk(title, lengthOfTalk);

            talk.Should().NotBeNull();
            talk.Title.Should().Be(title);
            talk.Duration.Should().Be(lengthOfTalk);
        }
    }
}
