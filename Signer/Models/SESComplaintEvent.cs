namespace AwsSigning.Models
{
    /// <summary>
    /// Event returned after the Simple Emailing Service received a complaint
    /// </summary>
    public class SESComplaintEvent : SESEvent
    {
        /// <summary>
        /// Containing information related to the bounce
        /// </summary>
        public Complaint Complaint { get; set; }
    }
}
