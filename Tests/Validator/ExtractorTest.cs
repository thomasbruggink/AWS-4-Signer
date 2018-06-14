using AwsSigning;
using AwsSigning.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Validator
{
    [TestClass]
    public class ExtractorTest
    {
        [TestMethod]
        public void TypeExtractTest()
        {
            var validator = new AwsValidator();
            var request = Helpers.CreateSubscriptionRequest();

            var expectedType = MessageType.SubscriptionConfirmation;
            var actual = validator.ParseMessageType(request);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestNotification()
        {
            var validator = new AwsValidator();
            var request = Helpers.CreateSubscriptionRequest();
            request.Headers.Remove("x-amz-sns-message-type");
            request.Headers.Add("x-amz-sns-message-type", "Notification");

            var expectedType = MessageType.Notification;
            var actual = validator.ParseMessageType(request);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestEmptyString()
        {
            var validator = new AwsValidator();
            var request = Helpers.CreateSubscriptionRequest();
            request.Headers.Remove("x-amz-sns-message-type");
            request.Headers.Add("x-amz-sns-message-type", string.Empty);

            var expectedType = MessageType.None;
            var actual = validator.ParseMessageType(request);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestRemoved()
        {
            var validator = new AwsValidator();
            var request = Helpers.CreateSubscriptionRequest();
            request.Headers.Remove("x-amz-sns-message-type");

            var expectedType = MessageType.None;
            var actual = validator.ParseMessageType(request);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestMultiple()
        {
            var validator = new AwsValidator();
            var request = Helpers.CreateSubscriptionRequest();
            request.Headers.Add("x-amz-sns-message-type", "Notificaton");

            var expectedType = MessageType.None;
            var actual = validator.ParseMessageType(request);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }
    }
}
