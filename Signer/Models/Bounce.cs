using System;
using System.Collections.Generic;

namespace AwsSigning.Models
{
    /// <summary>
    /// Containing information related to the bounce
    /// </summary>
    public class Bounce
    {
        /// <summary>
        /// The type of the bounce
        /// </summary>
        public string BounceType { get; set; }

        /// <summary>
        /// The subtype of the bounce
        /// </summary>
        public string BounceSubType { get; set; }

        /// <summary>
        /// List of recipients for which the email was bounced
        /// </summary>
        public IEnumerable<BouncedRecipient> BouncedRecipients { get; set; }

        /// <summary>
        /// Timestamp the bounce occured
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Amazon Identifier related to the feedback of the bounce
        /// </summary>
        public string FeedbackId { get; set; }

        /// <summary>
        /// The Mail Transfer Agent which proxied the email request
        /// </summary>
        public string ReportingMTA { get; set; }
    }
}
