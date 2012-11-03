using System;
using System.Linq;
using System.Globalization;
using ConferenceTracker.ConferenceManagement;
using ConferenceTracker.Infrastructure;
using ConferenceTracker.SessionManagement;
using ConferenceTracker.TalkManagement;
using ConferenceTracker.TrackManagement;
using CTMConsole.Utlities;

namespace CTMConsole
{
    /// <summary>
    /// A class that represents the main console program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            // Provide the user with the initial choices
            Program.CaptureAndProcessUserChoice();
        }

        /// <summary>
        /// Captures the user's choice.
        /// </summary>
        public static void CaptureAndProcessUserChoice()
        {
            // Display the choices
            Program.DisplayChoices();

            // Get the user's choice
            string chosenOption = Console.ReadLine();
            Console.WriteLine(Environment.NewLine);

            bool shouldTerminate = false;
            switch (chosenOption)
            {
                case "1":
                    // Choice 1 : Schedule the talk
                    Program.AddTalk();
                    break;
                case "2":
                    // Choice 2 : Display the schedule details of the conference
                    Program.DisplaySchedule();
                    break;
                case "3":
                    // Choice 3 : Exit the conference manager
                    shouldTerminate = true;
                    break;
            };

            // If user's choice has not been to exit, then display the user choices to continue again.
            if (!shouldTerminate)
            {
                Program.CaptureAndProcessUserChoice();
            }
        }

        /// <summary>
        /// Displays the choices.
        /// </summary>
        public static void DisplayChoices()
        {
            UIHelper.WriteLine("Please choose one of the options:", true);
            UIHelper.WriteLine("1 - Add a new Talk.", true);
            UIHelper.WriteLine("2 - See the conference schedule.", true);
            UIHelper.WriteLine("3 - Exit conference scheduler", true);
            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Adds the talk.
        /// </summary>
        public static void AddTalk()
        {
            // Get the user interaction data
            UserInterfaceTransactionData uiTransactionData = Program.GetUITransactionData();

            // Ask the conference manager to extract the talk from the user input
            // and schedule the same for the conference
            ConferenceManager.CreateScheduleFor(ref uiTransactionData);

            // Check whether the user input has any faults 
            // OR
            // whether the conference manager was unable to create a schedule
            if (uiTransactionData.HasFaults == false)
            {
                // Send out a user friendly message
                Console.WriteLine(Environment.NewLine);
                UIHelper.WriteLine("** The talk has been scheduled successfully!! **", true);
            }
            else
            {
                // Write the error(s) onto the console
                Console.WriteLine(Environment.NewLine);
                foreach (Fault oneError in uiTransactionData.Faults)
                {
                    UIHelper.WriteLine("** There has been some error!! **", true);
                    UIHelper.WriteLine(string.Format(CultureInfo.InvariantCulture, "Message : {0}", oneError.Message), true);
                    UIHelper.WriteLine(string.Format(CultureInfo.InvariantCulture, "Code : {0}", oneError.Code), true);
                }
            }
        }

        /// <summary>
        /// Displays the schedule.
        /// </summary>
        public static void DisplaySchedule()
        {
            int trackCount = 1;

            // For every available track,
            ConferenceManager.Tracks.ToList().ForEach(
                aTrack =>
                {
                    if (aTrack != null)
                    {
                        // Write the track header
                        Console.WriteLine(Environment.NewLine);
                        UIHelper.WriteMainHeader(
                            string.Format(CultureInfo.InvariantCulture, "Track {0} :", trackCount));

                        // For every session scheduled for the given track,
                        aTrack.Sessions.ToList().ForEach(
                            aSession =>
                            {
                                if (aSession != null)
                                {
                                    // Write the session headers
                                    UIHelper.WriteSubHeader(
                                        string.Format(CultureInfo.InvariantCulture, "Session {0} :", Enum.GetName(typeof(TimeOfDay), aSession.TimeOfDay)));

                                    // For every talk in the session
                                    foreach (Talk oneTalk in aSession.Talks)
                                    {
                                        // Write the talk details to the console
                                        UIHelper.WriteLine(
                                            string.Format(CultureInfo.InvariantCulture, "{0} : {1} mins", oneTalk.Title, oneTalk.Duration));
                                    }
                                }
                            });

                        trackCount++;
                    }
                });
        }

        /// <summary>
        /// Gets the UI transaction data.
        /// </summary>
        /// <returns>UserInterfaceTransactionData.</returns>
        private static UserInterfaceTransactionData GetUITransactionData()
        {
            // Ask the user for the talk details
            UIHelper.WriteLine("Please enter the talk title", true);
            string talkTitle = Console.ReadLine();
            UIHelper.WriteLine("", true);
            UIHelper.WriteLine("Please enter the duration/length of the talk", true);
            string lengthOfTalk = Console.ReadLine();

            // Return the instance of the user interaction data
            return new UserInterfaceTransactionData(talkTitle, lengthOfTalk);
        }
    }
}
