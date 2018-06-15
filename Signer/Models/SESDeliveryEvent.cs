namespace AwsSigning.Models
{
    /// <summary>
    /// Event returned after a succesful delivery by the Simple Emailing Service 
    /// </summary>
    public class SESDeliveryEvent : SESEvent
    {
        /// <summary>
        /// Containing information related to the bounce
        /// </summary>
        public Delivery Delivery { get; set; }
    }
}
