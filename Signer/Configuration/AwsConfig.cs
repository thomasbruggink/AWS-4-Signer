namespace AwsSigning.Configuration
{
    public class AwsConfig
    {
        /// <summary>
        /// The service of Amazon
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// The Amazon region
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Key for access with the Amazon API
        /// </summary>
        public string AccessId { get; set; }

        /// <summary>
        /// Key used as authentication with the Amazon API
        /// </summary>
        public string Secret { get; set; }
    }
}
