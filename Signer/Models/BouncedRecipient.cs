namespace AwsSigning.Models
{
    /// <summary>
    /// The recipient of an email for which a bounce occured
    /// </summary>
    public class BouncedRecipient : Recipient
    {
        /// <summary>
        /// Message containing information about why the bounce occured
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// SMTP Statuscode containing information about why the bounce occured
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Code returned by the Mail Transfer Agent on why he bounced the email
        /// </summary>
        public string DiagnosticCode { get; set; }
    }
}
