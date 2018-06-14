using System.Collections.Generic;
using System.Text;

namespace AwsSigning.Models
{
    /// <summary>
    /// When Amazon SNS sends a notification to a subscribed HTTP or HTTPS endpoint, the POST message sent to the endpoint has a message body that contains a JSON document with the following name/value pairs. 
    /// </summary>
    public class AwsNotification : AwsBase
    {
        /// <summary>
        /// The Subject parameter specified when the notification was published to the topic. Note that this is an optional parameter.
        /// If no Subject was specified, then this name/value pair does not appear in this JSON document. 
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// A URL that you can use to unsubscribe the endpoint from this topic.
        /// If you visit this URL, Amazon SNS unsubscribes the endpoint and stops sending notifications to this endpoint. 
        /// </summary>
        public string UnsubscribeURL { get; set; }

        public override string CreateSigningString()
        {
            var input = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Message", Message),
                new KeyValuePair<string, string>("MessageId", MessageId),
                new KeyValuePair<string, string>("Timestamp", Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
                new KeyValuePair<string, string>("TopicArn", TopicArn),
                new KeyValuePair<string, string>("Type", Type.ToString())
            };
            if(Subject != null)
                input.Add(new KeyValuePair<string, string>("Subject", Subject));
            return BuildSigningString(input);
        }
    }
}
