using System;
using System.Collections.Generic;

namespace AwsSigning.Models
{
    /// <summary>
    /// Containing information related to the bounce
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// The timestamp the delivery took place
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The time it took Simple Emailing Service to send the email
        /// </summary>
        public int ProcessingTimeMillis { get; set; }

        /// <summary>
        /// List of recipients to which the email was succesfully delivered
        /// </summary>
        public IEnumerable<string> Recipients { get; set; }

        /// <summary>
        /// Statuscode based on the SMTP protocol related to the delivery of the email
        /// </summary>
        public string SmtpResponse { get; set; }

        /// <summary>
        /// The Mail Transfer Agent which proxied the email request
        /// </summary>
        public string ReportingMTA { get; set; }
    }
}
