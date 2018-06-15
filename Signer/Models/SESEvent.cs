namespace AwsSigning.Models
{
    /// <summary>
    /// Event send by the Amazon Simple Notification System triggered by the Simple Emailing Service
    /// </summary>
    public class SESEvent
    {
        /// <summary>
        /// The type of SES event
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Information about the email related to this SES event
        /// </summary>
        public Mail Mail { get; set; }
    }
}
