using AwsSigning;
using AwsSigning.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Validator
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void NotificationParseTest()
        {
            var validator = new AwsValidator();
            var request = Helpers.CreateNotificationRequest();

            const string expectedSignatureVersion = "1";
            const string expectedSignature = "EXAMPLEw6JRNwm1LFQL4ICB0bnXrdB8ClRMTQFGBqwLpGbM78tJ4etTwC5zU7O3tS6tGpey3ejedNdOJ+1fkIp9F2/LmNVKb5aFlYq+9rk9ZiPph5YlLmWsDcyC5T+Sy9/umic5S0UQc2PEtgdpVBahwNOdMW4JPwk0kAJJztnc=";
            const string expectedSigningUrl = "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem";

            var result = validator.ParseContent(MessageType.Notification, request.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            Assert.AreEqual(typeof(AwsNotification), result.GetType(), "Notification parser failed");
            Assert.AreEqual(expectedSignatureVersion, result.SignatureVersion, "Notification parser failed");
            Assert.AreEqual(expectedSignature, result.Signature, "Notification parser failed");
            Assert.AreEqual(expectedSigningUrl, result.SigningCertURL, "Notification parser failed");
        }
    }
}
