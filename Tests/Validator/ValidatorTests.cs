using System.IO;
using AwsSigning.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Validator
{
    /// <summary>
    /// Validate or the received callback from the Amazon Simple Notification Service is actually from Amazon and isn't tempered with
    /// https://docs.aws.amazon.com/sns/latest/dg/SendMessageToHttp.verify.signature.html
    /// </summary>
    /// <returns></returns>
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void VerifyValidNotificationRequest()
        {
            var body = new MemoryStream(Helpers.CreateNotificationBody());
            var headers = Helpers.CreateNotificationHeaders();

            Assert.IsTrue(AwsValidator.Verify(body, headers).GetAwaiter().GetResult(), "Verify notification request should succeed");
        }

        [TestMethod]
        public void VerifyValidSubscriptionRequest()
        {
            var body = new MemoryStream(Helpers.CreateSubscriptionBody());
            var headers = Helpers.CreateSubscriptionHeaders();

            Assert.IsTrue(AwsValidator.Verify(body, headers).GetAwaiter().GetResult(), "Verify subscription request should succeed");
        }
    }
}
