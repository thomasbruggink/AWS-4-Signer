using System;
using System.Collections.Generic;

namespace AwsSigning.Models
{
    /// <summary>
    /// Information about an email to which a SES event is related
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// The timestamp the email is sent
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// The source of the email containing the name and email address
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The source of the email containing information related to Amazon like the region
        /// </summary>
        public string SourceArn { get; set; }

        /// <summary>
        /// The identifier of the Amazon account the email was sent from
        /// </summary>
        public string SendingAccountId { get; set; }

        /// <summary>
        /// The identifier of the message in Amazon
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// List of destination email addresses
        /// </summary>
        public IEnumerable<string> Destination { get; set; }

        /// <summary>
        /// Flag indicating whether the headers were shortened
        /// </summary>
        public bool HeadersTruncated { get; set; }

        /// <summary>
        /// List of key-value pair headers
        /// </summary>
        public IEnumerable<Header> Headers { get; set; }

        /// <summary>
        /// Containing common headers for an email
        /// </summary>
        public CommonHeaders CommonHeaders { get; set; }

        /// <summary>
        /// List of key-value tags where the value is a list of values
        /// The keys of each header contain a prefix with the related Amazon service
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Tags { get; set; }
    }
}
