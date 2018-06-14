using System.Text;
using AwsSigning;
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
            expectedBuilder.Append("Hello world!\n");
            expectedBuilder.Append("MessageId\n");
            expectedBuilder.Append("22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324\n");
            expectedBuilder.Append("Subject\n");
            expectedBuilder.Append("My First Message\n");
            expectedBuilder.Append("Timestamp\n");
            expectedBuilder.Append("2012-05-02T00:54:06.655Z\n");
            expectedBuilder.Append("TopicArn\n");
            expectedBuilder.Append("arn:aws:sns:us-west-2:123456789012:MyTopic\n");
            expectedBuilder.Append("Type\n");
            expectedBuilder.Append("Notification");

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
            expectedBuilder.Append("You have chosen to subscribe to the topic arn:aws:sns:us-west-2:123456789012:MyTopic.\nTo confirm the subscription, visit the SubscribeURL included in this message.\n");
            expectedBuilder.Append("MessageId\n");
            expectedBuilder.Append("165545c9-2a5c-472c-8df2-7ff2be2b3b1b\n");
            expectedBuilder.Append("SubscribeURL\n");
            expectedBuilder.Append("https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37fb687f5d51e6e241d09c805a5a57b30d712f794cc5f6a988666d92768dd60a747ba6f3beb71854e285d6ad02428b09ceece29417f1f02d609c582afbacc99c583a916b9981dd2728f4ae6fdb82efd087cc3b7849e05798d2d2785c03b0879594eeac82c01f235d0e717736\n");
            expectedBuilder.Append("Timestamp\n");
            expectedBuilder.Append("2012-04-26T20:45:04.751Z\n");
            expectedBuilder.Append("TopicArn\n");
            expectedBuilder.Append("arn:aws:sns:us-west-2:123456789012:MyTopic\n");
            expectedBuilder.Append("Type\n");
            expectedBuilder.Append("SubscriptionConfirmation");

            var content = validator.ParseContent(MessageType.SubscriptionConfirmation, Helpers.SubscriptionContent);
            var result = validator.BuildSigningString(content);

            Assert.AreEqual(expectedBuilder.ToString(), result, "SubscriptionConfirmation Signing string failed");
        }
    }
}
