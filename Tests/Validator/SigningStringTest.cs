using System.Text;
using AwsSigning.Helpers;
using AwsSigning.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Validator
{
    [TestClass]
    public class SigningStringTest
    {
        [TestMethod]
        public void NotificationSigningStringTest()
        {
            var validator = new AwsValidator();

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("Message\n");
            expectedBuilder.Append("{\"eventType\":\"Delivery\",\"mail\":{\"timestamp\":\"2018-06-06T12:39:12.765Z\",\"source\":\"paul.vanderbijl@infosupport.com\",\"sourceArn\":\"arn:aws:ses:eu-west-1:138286856132:identity/paul.vanderbijl@infosupport.com\",\"sendingAccountId\":\"138286856132\",\"messageId\":\"01020163d51bb47d-426d5135-0d53-4e67-87d0-27d654d5d3cf-000000\",\"destination\":[\"maikel.stuivenberg@infosupport.com\",\"paul.vanderbijl@infosupport.com\"],\"headersTruncated\":false,\"headers\":[{\"name\":\"From\",\"value\":\"paul.vanderbijl@infosupport.com\"},{\"name\":\"To\",\"value\":\"paul.vanderbijl@infosupport.com, maikel.stuivenberg@infosupport.com\"},{\"name\":\"Subject\",\"value\":\"AmazonMail\"},{\"name\":\"MIME-Version\",\"value\":\"1.0\"},{\"name\":\"Content-Type\",\"value\":\"text/plain; charset=UTF-8\"},{\"name\":\"Content-Transfer-Encoding\",\"value\":\"7bit\"}],\"commonHeaders\":{\"from\":[\"paul.vanderbijl@infosupport.com\"],\"to\":[\"paul.vanderbijl@infosupport.com\",\"maikel.stuivenberg@infosupport.com\"],\"messageId\":\"01020163d51bb47d-426d5135-0d53-4e67-87d0-27d654d5d3cf-000000\",\"subject\":\"AmazonMail\"},\"tags\":{\"ses:configuration-set\":[\"Callback\"],\"ses:source-ip\":[\"213.154.239.254\"],\"ses:from-domain\":[\"infosupport.com\"],\"ses:caller-identity\":[\"paul\"],\"tenant\":[\"twygger\"],\"ses:outgoing-ip\":[\"54.240.7.11\"]}},\"delivery\":{\"timestamp\":\"2018-06-06T12:39:13.742Z\",\"processingTimeMillis\":977,\"recipients\":[\"maikel.stuivenberg@infosupport.com\",\"paul.vanderbijl@infosupport.com\"],\"smtpResponse\":\"250 ok 1528288753 qp 14158 server-6.tower-230.messagelabs.com!1528288753!1170990!1\",\"reportingMTA\":\"a7-11.smtp-out.eu-west-1.amazonses.com\"}}\n\n");
            expectedBuilder.Append("MessageId\n");
            expectedBuilder.Append("77153aac-352b-5f4c-a164-693ded1a1607\n");
            expectedBuilder.Append("Subject\n");
            expectedBuilder.Append("Amazon SES Email Event Notification\n");
            expectedBuilder.Append("Timestamp\n");
            expectedBuilder.Append("2018-06-06T12:39:13.865Z\n");
            expectedBuilder.Append("TopicArn\n");
            expectedBuilder.Append("arn:aws:sns:eu-west-1:138286856132:EmailWebhook\n");
            expectedBuilder.Append("Type\n");
            expectedBuilder.Append("Notification\n");

            var content = validator.ParseContent(MessageType.Notification, Helpers.NotificationContent);
            var result = validator.BuildSigningString(content);

            Assert.AreEqual(expectedBuilder.ToString(), result, "Notification Signing string failed");
        }

        [TestMethod]
        public void SubscriptionSigningStringTest()
        {
            var validator = new AwsValidator();

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("Message\n");
            expectedBuilder.Append("You have chosen to subscribe to the topic arn:aws:sns:eu-west-1:138286856132:EmailWebhook.\nTo confirm the subscriptionDataBuilder, visit the SubscribeURL included in this message.\n");
            expectedBuilder.Append("MessageId\n");
            expectedBuilder.Append("3d4186f0-808e-4e2a-a3aa-79f6a95530c5\n");
            expectedBuilder.Append("SubscribeURL\n");
            expectedBuilder.Append("https://sns.eu-west-1.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:eu-west-1:138286856132:EmailWebhook&Token=2336412f37fb687f5d51e6e241da92fd739f58c44a4a1b9b163abcd8cd04eb9cd300541e6f85e09df3f9672fbde2f0e73b63610e602435cbff9a08a84a53104edbef275c2e2e61aeac9be4da44073fc7275e045191d125aa1c1c9c9dfdee64f7dd3b13c77670f1037843a9c6f2d05cc7\n");
            expectedBuilder.Append("Timestamp\n");
            expectedBuilder.Append("2018-06-06T12:38:33.131Z\n");
            expectedBuilder.Append("Token\n");
            expectedBuilder.Append("2336412f37fb687f5d51e6e241da92fd739f58c44a4a1b9b163abcd8cd04eb9cd300541e6f85e09df3f9672fbde2f0e73b63610e602435cbff9a08a84a53104edbef275c2e2e61aeac9be4da44073fc7275e045191d125aa1c1c9c9dfdee64f7dd3b13c77670f1037843a9c6f2d05cc7\n");
            expectedBuilder.Append("TopicArn\n");
            expectedBuilder.Append("arn:aws:sns:eu-west-1:138286856132:EmailWebhook\n");
            expectedBuilder.Append("Type\n");
            expectedBuilder.Append("SubscriptionConfirmation\n");

            var content = validator.ParseContent(MessageType.SubscriptionConfirmation, Helpers.SubscriptionContent);
            var result = validator.BuildSigningString(content);

            Assert.AreEqual(expectedBuilder.ToString(), result, "SubscriptionConfirmation Signing string failed");
        }
    }
}
