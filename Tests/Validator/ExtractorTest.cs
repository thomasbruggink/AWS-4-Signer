using AwsSigning.Helpers;
using AwsSigning.Models;
using Microsoft.AspNetCore.Http;
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
            var headers = Helpers.CreateSubscriptionHeaders();

            var expectedType = MessageType.SubscriptionConfirmation;
            var actual = validator.ParseMessageType(headers);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestNotification()
        {
            var validator = new AwsValidator();
            var headers = Helpers.CreateSubscriptionHeaders();
            headers.Remove("x-amz-sns-message-type");
            headers.Add("x-amz-sns-message-type", "Notification");

            var expectedType = MessageType.Notification;
            var actual = validator.ParseMessageType(headers);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestEmptyString()
        {
            var validator = new AwsValidator();
            var headers = Helpers.CreateSubscriptionHeaders();
            headers.Remove("x-amz-sns-message-type");
            headers.Add("x-amz-sns-message-type", string.Empty);

            var expectedType = MessageType.None;
            var actual = validator.ParseMessageType(headers);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestRemoved()
        {
            var validator = new AwsValidator();
            var headers = Helpers.CreateSubscriptionHeaders();
            headers.Remove("x-amz-sns-message-type");

            var expectedType = MessageType.None;
            var actual = validator.ParseMessageType(headers);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }

        [TestMethod]
        public void TypeExtractTestMultiple()
        {
            var validator = new AwsValidator();
            var headers = Helpers.CreateSubscriptionHeaders();
            headers.Append("x-amz-sns-message-type", "Notificaton");

            var expectedType = MessageType.None;
            var actual = validator.ParseMessageType(headers);

            Assert.AreEqual(expectedType, actual, "Type Extractor failed");
        }
    }
}
