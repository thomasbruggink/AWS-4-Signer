namespace AwsSigning.Models
{
    /// <summary>
    /// Event returned after an email send via the Simple Emailing Service was rejected
    /// </summary>
    public class SESRejectEvent : SESEvent
    {
        /// <summary>
        /// Containing information related to the reject
        /// </summary>
        public Reject Reject { get; set; }
    }
}
