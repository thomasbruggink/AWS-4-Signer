using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Tests.Validator
{
    class Helpers
    {
        public static string SubscriptionContent = "{\r\n  \"Type\" : \"SubscriptionConfirmation\",\r\n  \"MessageId\" : \"165545c9-2a5c-472c-8df2-7ff2be2b3b1b\",\r\n  \"Token\" : \"2336412f37fb687f5d51e6e241d09c805a5a57b30d712f794cc5f6a988666d92768dd60a747ba6f3beb71854e285d6ad02428b09ceece29417f1f02d609c582afbacc99c583a916b9981dd2728f4ae6fdb82efd087cc3b7849e05798d2d2785c03b0879594eeac82c01f235d0e717736\",\r\n  \"TopicArn\" : \"arn:aws:sns:us-west-2:123456789012:MyTopic\",\r\n  \"Message\" : \"You have chosen to subscribe to the topic arn:aws:sns:us-west-2:123456789012:MyTopic.\\nTo confirm the subscription, visit the SubscribeURL included in this message.\",\r\n  \"SubscribeURL\" : \"https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37fb687f5d51e6e241d09c805a5a57b30d712f794cc5f6a988666d92768dd60a747ba6f3beb71854e285d6ad02428b09ceece29417f1f02d609c582afbacc99c583a916b9981dd2728f4ae6fdb82efd087cc3b7849e05798d2d2785c03b0879594eeac82c01f235d0e717736\",\r\n  \"Timestamp\" : \"2012-04-26T20:45:04.751Z\",\r\n  \"SignatureVersion\" : \"1\",\r\n  \"Signature\" : \"EXAMPLEpH+DcEwjAPg8O9mY8dReBSwksfg2S7WKQcikcNKWLQjwu6A4VbeS0QHVCkhRS7fUQvi2egU3N858fiTDN6bkkOxYDVrY0Ad8L10Hs3zH81mtnPk5uvvolIC1CXGu43obcgFxeL3khZl8IKvO61GWB6jI9b5+gLPoBc1Q=\",\r\n  \"SigningCertURL\" : \"https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem\"\r\n  }";
        public static HttpRequestMessage CreateSubscriptionRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(SubscriptionContent, Encoding.UTF8, "text/plain")
            };
            request.Headers.Add("User-Agent", "Amazon Simple Notification Service Agent");
            request.Headers.Add("x-amz-sns-message-type", "SubscriptionConfirmation");
            request.Headers.Add("x-amz-sns-message-id", "165545c9-2a5c-472c-8df2-7ff2be2b3b1b");
            request.Headers.Add("x-amz-sns-topic-arn", "arn:aws:sns:us-west-2:123456789012:MyTopic");
            return request;
        }

        public static string NotificationContent = "{\r\n  \"Type\" : \"Notification\",\r\n  \"MessageId\" : \"22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324\",\r\n  \"TopicArn\" : \"arn:aws:sns:us-west-2:123456789012:MyTopic\",\r\n  \"Subject\" : \"My First Message\",\r\n  \"Message\" : \"Hello world!\",\r\n  \"Timestamp\" : \"2012-05-02T00:54:06.655Z\",\r\n  \"SignatureVersion\" : \"1\",\r\n  \"Signature\" : \"EXAMPLEw6JRNwm1LFQL4ICB0bnXrdB8ClRMTQFGBqwLpGbM78tJ4etTwC5zU7O3tS6tGpey3ejedNdOJ+1fkIp9F2/LmNVKb5aFlYq+9rk9ZiPph5YlLmWsDcyC5T+Sy9/umic5S0UQc2PEtgdpVBahwNOdMW4JPwk0kAJJztnc=\",\r\n  \"SigningCertURL\" : \"https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem\",\r\n  \"UnsubscribeURL\" : \"https://sns.us-west-2.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:us-west-2:123456789012:MyTopic:c9135db0-26c4-47ec-8998-413945fb5a96\"\r\n}";
        public static HttpRequestMessage CreateNotificationRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(NotificationContent, Encoding.UTF8, "text/plain")
            };
            request.Headers.Add("User-Agent", "Amazon Simple Notification Service Agent");
            request.Headers.Add("x-amz-sns-message-type", "Notification");
            request.Headers.Add("x-amz-sns-message-id", "22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324");
            request.Headers.Add("x-amz-sns-topic-arn", "arn:aws:sns:us-west-2:123456789012:MyTopic");
            request.Headers.Add("x-amz-sns-subscription-arn", "arn:aws:sns:us-west-2:123456789012:MyTopic:c9135db0-26c4-47ec-8998-413945fb5a96");
            return request;
        }
    }
}
