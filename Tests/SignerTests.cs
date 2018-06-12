using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwsSigning;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SignerTests
    {
        /// <summary>
        /// https://docs.aws.amazon.com/general/latest/gr/sigv4-create-string-to-sign.html
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateSignString()
        {
            var signer = new AwsSigner();
            var request = Helpers.CreateExampleRequest();

            var region = "us-east-1";
            var service = "iam";
            var dateTime = DateTimeOffset.Parse("2015-08-30T12:36:00Z");

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("AWS4-HMAC-SHA256\n");
            expectedBuilder.Append("20150830T123600Z\n");
            expectedBuilder.Append("20150830/us-east-1/iam/aws4_request\n");
            expectedBuilder.Append("f536975d06c0309214f805bb90ccff089219ecd68b2577efef23edd43b7e1a59");

            var output = await signer.CreateSignString(region, service, request, dateTime);
            Assert.AreEqual(expectedBuilder.ToString(), output, "Create Sign string failed");
        }

        [TestMethod]
        public void CreateSigningKey()
        {
            var signer = new AwsSigner();
            var config = Helpers.CreateExampleConfig();
            const string expected = "c4afb1cc5771d871763a393e44b703571b55cc28424d1a5e86da6ed3c154a4b9";
            var dateTime = DateTimeOffset.Parse("2015-08-30T12:36:00Z");

            var result = signer.CreateSigningKey(config.Secret, dateTime, config.Region, config.Service);
            // Convert the result to hex string just to compare
            // In the actual world this stays a byte array
            Assert.AreEqual(expected, signer.ToHexString(result), "Create signing key failed");
        }

        /// <summary>
        /// https://docs.aws.amazon.com/general/latest/gr/sigv4-calculate-signature.html
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateSignature()
        {
            var signer = new AwsSigner();
            var config = Helpers.CreateExampleConfig();
            var request = Helpers.CreateExampleRequest();
            const string expected = "5d672d79c15b13162d9279b0855cfba6789a8edb4c82c400e06b5924a6f2b5d7";
            var dateTime = DateTimeOffset.Parse("2015-08-30T12:36:00Z");

            var signingKey = signer.CreateSigningKey(config.Secret, dateTime, config.Region, config.Service);
            var signString = await signer.CreateSignString(config.Region, config.Service, request, dateTime);
            var result = signer.CreateSignature(signingKey, signString);
            Assert.AreEqual(expected, result, "Create signature failed");
        }
    }
}
