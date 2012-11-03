using System.Collections.Generic;

namespace ConferenceTracker.Infrastructure
{
    /// <summary>
    /// A class that carries input from the user
    /// </summary>
    public class UserInterfaceTransactionData
    {
        /// <summary>
        /// The list of faults
        /// </summary>
        private List<Fault> _fault;

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        /// <value>The input string.</value>
        public string TalkTitle { get; private set; }

        /// <summary>
        /// Gets the length of talk.
        /// </summary>
        /// <value>The length of talk.</value>
        public string LengthOfTalk { get; private set; }

        /// <summary>
        /// Gets or sets the faults.
        /// </summary>
        /// <value>The faults.</value>
        public List<Fault> Faults
        {
            get
            {
                if (_fault == null)
                {
                    _fault = new List<Fault>();
                }
                return _fault;
            }
            set
            {
                _fault = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has faults.
        /// </summary>
        /// <value><c>true</c> if this instance has faults; otherwise, <c>false</c>.</value>
        public bool HasFaults
        {
            get
            {
                return (this.Faults != null && this.Faults.Count > 0);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceTransactionData" /> class.
        /// </summary>
        /// <param name="talkTitle">The talk title.</param>
        /// <param name="durationOfTalk">The duration of talk.</param>
        public UserInterfaceTransactionData(string talkTitle, string durationOfTalk)
        {
            this.TalkTitle = talkTitle;
            this.LengthOfTalk = durationOfTalk;
        }
    }
}
