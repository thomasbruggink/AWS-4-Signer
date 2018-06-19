using System.Text;
using AwsSigning.Helpers;
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
            var content = Encoding.UTF8.GetString(Helpers.CreateNotificationBody());

            const string expectedSignatureVersion = "1";
            const string expectedSignature = "EHfWWFB3hXW6kYukANmxNgrcipip/N0m4xPzbsZwtAVGL3tjlq+PXMEsuFFyd9ctR/sNf1rBeh690dvn8byG2WQ/DyaxpAm35yJvDvRaZIMJUOAEH97OEsH++dw3/2LOIb3AL0EDegY3/9/1jDmM+aHxCjziopCG8f9uEzZOpZ03SFxUQ9ZLrse0xZWvtVKSiSU23YzHrzhb44nLTYtonu9Xq5xm3xaRR9CwoXxZbQURLYWqVHyDWDH0cRxdcNW6s/1XpBQP3pzxh6KZs99uiAzL4w5rr05lqhOQGzPgfolI6n1qJJFTY4zPnzJhBKtfrqLikKCNFeg2UACr4dnN4Q==";
            const string expectedSigningUrl = "https://sns.eu-west-1.amazonaws.com/SimpleNotificationService-eaea6120e66ea12e88dcd8bcbddca752.pem";

            var result = validator.ParseContent(MessageType.Notification, content);
            Assert.AreEqual(typeof(AwsNotification), result.GetType(), "Notification parser failed");
            Assert.AreEqual(expectedSignatureVersion, result.SignatureVersion, "Notification parser failed");
            Assert.AreEqual(expectedSignature, result.Signature, "Notification parser failed");
            Assert.AreEqual(expectedSigningUrl, result.SigningCertURL, "Notification parser failed");
        }
    }
}
