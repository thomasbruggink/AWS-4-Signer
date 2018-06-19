using System;

namespace AwsSigning.Models
{
    /// <summary>
    /// An event send by the Amazon Simple notification Service
    /// </summary>
    public class SnSEvent
    {
        /// <summary>
        /// The type of event, for example: Notification
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Contains information about the service from which this event originates
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The time this event was sent
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The unique identifier of this message
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Contains the actual event in a json serialized format
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Reference data for the topic related to this event
        /// </summary>
        public string TopicArn { get; set; }

        /// <summary>
        /// Signature send with the request to validate this request originates from Amazon
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Version which was used to generate the signature
        /// Currently only version 1 is supported: https://docs.aws.amazon.com/sns/latest/dg/SendMessageToHttp.verify.signature.html
        /// </summary>
        public string SignatureVersion { get; set; }

        /// <summary>
        /// Url to confirm the subscription of this controller to the Amazon Simple Notification System
        /// </summary>
        public string SubscribeURL { get; set; }

        /// <summary>
        /// Token to verify the origin of the request
        /// </summary>
        public string Token { get; set; }
    }
}
