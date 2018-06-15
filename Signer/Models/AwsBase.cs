using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwsSigning.Models
{
    /// <summary>
    /// The list of possible message types AWS can send
    /// </summary>
    public enum MessageType
    {
        None,
        SubscriptionConfirmation,
        Notification,
        UnsubscribeConfirmation
    }

    public abstract class AwsBase
    {
        /// <summary>
        /// The type of message.
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// A Universally Unique Identifier, unique for each message published. For a message that Amazon SNS resends during a retry, the message ID of the original message is used. 
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// The Amazon Resource Name (ARN) for the topic that this endpoint has been unsubscribed from. 
        /// </summary>
        public string TopicArn { get; set; }
        /// <summary>
        /// A string that describes the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The time (GMT) when the reqeust was sent.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Version of the Amazon SNS signature used.
        /// </summary>
        public string SignatureVersion { get; set; }
        /// <summary>
        /// Base64-encoded "SHA1withRSA" signature of the Message, MessageId, Type, Timestamp, and TopicArn values. 
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// The URL to the certificate that was used to sign the message.
        /// </summary>
        public string SigningCertURL { get; set; }

        /// <summary>
        /// Create the signing string
        /// </summary>
        /// <returns></returns>
        public abstract string CreateSigningString();

        protected string BuildSigningString(List<KeyValuePair<string, string>> input)
        {
            var output = new List<string>();
            foreach (var keyValuePair in input.OrderBy(kv => kv.Key, StringComparer.Ordinal))
            {
                output.Add(keyValuePair.Key);
                output.Add(keyValuePair.Value);
            }
            return string.Join('\n', output) + '\n';
        }
    }
}
