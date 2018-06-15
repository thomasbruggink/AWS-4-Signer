namespace AwsSigning.Models
{
    /// <summary>
    /// Raw header sent with the request
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Name of the header
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Value of the header
        /// </summary>
        public string Value { get; set; }
    }
}
