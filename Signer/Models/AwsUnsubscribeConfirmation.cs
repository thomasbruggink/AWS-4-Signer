using System.Collections.Generic;
using System.Text;

namespace AwsSigning.Models
{
    /// <summary>
    /// After an HTTP/HTTPS endpoint is unsubscribed from a topic, Amazon SNS sends an unsubscribe confirmation message to the endpoint. 
    /// </summary>
    public class AwsUnsubscribeConfirmation : AwsBase
    {
        /// <summary>
        /// A value you can use with the ConfirmSubscription action to re-confirm the subscription. Alternatively, you can simply visit the SubscribeURL. 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The URL that you must visit in order to re-confirm the subscription. Alternatively, you can instead use the Token with the ConfirmSubscription action to re-confirm the subscription. 
        /// </summary>
        public string SubscribeURL { get; set; }

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
            if (SubscribeURL != null)
                input.Add(new KeyValuePair<string, string>("SubscribeURL", SubscribeURL));
            return BuildSigningString(input);
        }
    }
}
