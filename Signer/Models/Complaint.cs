using System;
using System.Collections.Generic;

namespace AwsSigning.Models
{
    /// <summary>
    /// Containing information related to the complaint
    /// </summary>
    public class Complaint
    {
        /// <summary>
        /// List of the recipients for which a complaint is filed
        /// </summary>
        public IEnumerable<Recipient> ComplainedRecipients { get; set; }

        /// <summary>
        /// The time the complain was handled
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Amazon Identifier related to the feedback of the complaint
        /// </summary>
        public string FeedbackId { get; set; }

        /// <summary>
        /// Containing information about the client who sent the complaint to the email provided
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Information about why the complaint was filed
        /// </summary>
        public string ComplaintFeedbackType { get; set; }

        /// <summary>
        /// The time the complaint was filed
        /// </summary>
        public DateTimeOffset ArrivalDate { get; set; }
    }
}
