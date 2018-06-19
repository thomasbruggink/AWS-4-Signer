using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests.Validator
{
    class Helpers
    {
        public static string SubscriptionContent => JsonConvert.SerializeObject(new JObject
        {
            {"Type" , "SubscriptionConfirmation"},
            {"MessageId",  "3d4186f0-808e-4e2a-a3aa-79f6a95530c5"},
            {"Token" , "2336412f37fb687f5d51e6e241da92fd739f58c44a4a1b9b163abcd8cd04eb9cd300541e6f85e09df3f9672fbde2f0e73b63610e602435cbff9a08a84a53104edbef275c2e2e61aeac9be4da44073fc7275e045191d125aa1c1c9c9dfdee64f7dd3b13c77670f1037843a9c6f2d05cc7"},
            {"TopicArn" , "arn:aws:sns:eu-west-1:138286856132:EmailWebhook"},
            {"Message" , "You have chosen to subscribe to the topic arn:aws:sns:eu-west-1:138286856132:EmailWebhook.\nTo confirm the subscriptionDataBuilder, visit the SubscribeURL included in this message."},
            {"SubscribeURL" , "https://sns.eu-west-1.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:eu-west-1:138286856132:EmailWebhook&Token=2336412f37fb687f5d51e6e241da92fd739f58c44a4a1b9b163abcd8cd04eb9cd300541e6f85e09df3f9672fbde2f0e73b63610e602435cbff9a08a84a53104edbef275c2e2e61aeac9be4da44073fc7275e045191d125aa1c1c9c9dfdee64f7dd3b13c77670f1037843a9c6f2d05cc7"},
            {"Timestamp" , "2018-06-06T12:38:33.131Z"},
            {"SignatureVersion" , "1"},
            {"Signature" , "DZOET8m+aBAn7tcKK/jPEIxZ3kLZjBvjP1NZocfX10R5B1Ud1YUVHKgm2t5Yvm1kkwusyIFhDSdCqNovCgb7IcpiqHWPi+1yHhJYBatPvqLOk8HVJ1zc0aifWKurRlwMmXPX86fQUWEuD2AmzY/1FfsWyT0yB/lVyPQQMOlSA5VhDHiw2UaTf0P5lpEU4gaonDiNAODC7FeljpeMi8Cn1+NCWGla4YVEvreb7Mj1DJGt2UE5mwzff5ALGnQKv5+6TL6BR8vNHeaerBJOq53RoykBNU51cA5uwG1tEpwyucSD3ipIGIjLTZ2JCIOf3Ovypp/Eg7BOVBzHkUaKNxIY/Q=="},
            {"SigningCertURL" , "https://sns.eu-west-1.amazonaws.com/SimpleNotificationService-eaea6120e66ea12e88dcd8bcbddca752.pem"}
        });
        public static byte[] CreateSubscriptionBody()
        {
            return Encoding.UTF8.GetBytes(SubscriptionContent);
        }

        public static IHeaderDictionary CreateSubscriptionHeaders()
        {
            return new HeaderDictionary
            {
                {"User-Agent", "Amazon Simple Notification Service Agent"},
                {"x-amz-sns-message-type", "SubscriptionConfirmation"},
                {"x-amz-sns-message-id", "3d4186f0-808e-4e2a-a3aa-79f6a95530c5"},
                {"x-amz-sns-topic-arn", "arn:aws:sns:eu-west-1:138286856132:EmailWebhook"}
            };
        }

        public static string NotificationContent => JsonConvert.SerializeObject(new JObject
        {
          {"Type" , "Notification"},
          {"MessageId" , "77153aac-352b-5f4c-a164-693ded1a1607"},
          {"TopicArn" , "arn:aws:sns:eu-west-1:138286856132:EmailWebhook"},
          {"Subject" , "Amazon SES Email Event Notification"},
          {"Message" , "{\"eventType\":\"Delivery\",\"mail\":{\"timestamp\":\"2018-06-06T12:39:12.765Z\",\"source\":\"paul.vanderbijl@infosupport.com\",\"sourceArn\":\"arn:aws:ses:eu-west-1:138286856132:identity/paul.vanderbijl@infosupport.com\",\"sendingAccountId\":\"138286856132\",\"messageId\":\"01020163d51bb47d-426d5135-0d53-4e67-87d0-27d654d5d3cf-000000\",\"destination\":[\"maikel.stuivenberg@infosupport.com\",\"paul.vanderbijl@infosupport.com\"],\"headersTruncated\":false,\"headers\":[{\"name\":\"From\",\"value\":\"paul.vanderbijl@infosupport.com\"},{\"name\":\"To\",\"value\":\"paul.vanderbijl@infosupport.com, maikel.stuivenberg@infosupport.com\"},{\"name\":\"Subject\",\"value\":\"AmazonMail\"},{\"name\":\"MIME-Version\",\"value\":\"1.0\"},{\"name\":\"Content-Type\",\"value\":\"text/plain; charset=UTF-8\"},{\"name\":\"Content-Transfer-Encoding\",\"value\":\"7bit\"}],\"commonHeaders\":{\"from\":[\"paul.vanderbijl@infosupport.com\"],\"to\":[\"paul.vanderbijl@infosupport.com\",\"maikel.stuivenberg@infosupport.com\"],\"messageId\":\"01020163d51bb47d-426d5135-0d53-4e67-87d0-27d654d5d3cf-000000\",\"subject\":\"AmazonMail\"},\"tags\":{\"ses:configuration-set\":[\"Callback\"],\"ses:source-ip\":[\"213.154.239.254\"],\"ses:from-domain\":[\"infosupport.com\"],\"ses:caller-identity\":[\"paul\"],\"tenant\":[\"twygger\"],\"ses:outgoing-ip\":[\"54.240.7.11\"]}},\"delivery\":{\"timestamp\":\"2018-06-06T12:39:13.742Z\",\"processingTimeMillis\":977,\"recipients\":[\"maikel.stuivenberg@infosupport.com\",\"paul.vanderbijl@infosupport.com\"],\"smtpResponse\":\"250 ok 1528288753 qp 14158 server-6.tower-230.messagelabs.com!1528288753!1170990!1\",\"reportingMTA\":\"a7-11.smtp-out.eu-west-1.amazonses.com\"}}\n"},
          {"Timestamp" , "2018-06-06T12:39:13.865Z"},
          {"SignatureVersion" , "1"},
          {"Signature" , "EHfWWFB3hXW6kYukANmxNgrcipip/N0m4xPzbsZwtAVGL3tjlq+PXMEsuFFyd9ctR/sNf1rBeh690dvn8byG2WQ/DyaxpAm35yJvDvRaZIMJUOAEH97OEsH++dw3/2LOIb3AL0EDegY3/9/1jDmM+aHxCjziopCG8f9uEzZOpZ03SFxUQ9ZLrse0xZWvtVKSiSU23YzHrzhb44nLTYtonu9Xq5xm3xaRR9CwoXxZbQURLYWqVHyDWDH0cRxdcNW6s/1XpBQP3pzxh6KZs99uiAzL4w5rr05lqhOQGzPgfolI6n1qJJFTY4zPnzJhBKtfrqLikKCNFeg2UACr4dnN4Q=="},
          {"SigningCertURL" , "https://sns.eu-west-1.amazonaws.com/SimpleNotificationService-eaea6120e66ea12e88dcd8bcbddca752.pem"},
          {"UnsubscribeURL" , "https://sns.eu-west-1.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:eu-west-1:138286856132:EmailWebhook:8f07a12d-0940-4839-930a-5d4a15092e86"}
        });

        public static byte[] CreateNotificationBody() {         
            return Encoding.UTF8.GetBytes(NotificationContent);
        }

        public static IHeaderDictionary CreateNotificationHeaders()
        {
            return new HeaderDictionary
            {
                {"User-Agent", "Amazon Simple Notification Service Agent"},
                {"x-amz-sns-message-type", "Notification"},
                {"x-amz-sns-message-id", "77153aac-352b-5f4c-a164-693ded1a1607"},
                {"x-amz-sns-topic-arn", "arn:aws:sns:eu-west-1:138286856132:EmailWebhook"},
                {"x-amz-sns-subscription-arn", "arn:aws:sns:eu-west-1:138286856132:EmailWebhook:8f07a12d-0940-4839-930a-5d4a15092e86"}
            };
        }

        public static Stream CreateStreamFromObject(JObject @object)
        {
            var serializedObject = JsonConvert.SerializeObject(@object);
            return new MemoryStream(Encoding.UTF8.GetBytes(serializedObject));
        }
    }
}
