namespace AwsSigning.Models
{
    /// <summary>
    /// Event returned after the Simple Emailing Service received a bounce
    /// </summary>
    public class SESBounceEvent : SESEvent
    {
        /// <summary>
        /// Containing information related to the bounce
        /// </summary>
        public Bounce Bounce { get; set; }
    }
}
