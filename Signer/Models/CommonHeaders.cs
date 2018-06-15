using System.Collections.Generic;

namespace AwsSigning.Models
{
    /// <summary>
    /// Commonly used headers of an email
    /// </summary>
    public class CommonHeaders
    {
        /// <summary>
        /// The headers containing the 'from' information
        /// </summary>
        public IEnumerable<string> From { get; set; }

        /// <summary>
        /// The headers containing the 'to' information
        /// </summary>
        public IEnumerable<string> To { get; set; }

        /// <summary>
        /// The identifier of the message
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// The subject of the message
        /// </summary>
        public string Subject { get; set; }
    }
}
