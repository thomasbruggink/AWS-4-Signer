using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwsSigning.Configuration;
using AwsSigning.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Signer
{
    /// <summary>
    /// https://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html
    /// </summary>
    [TestClass]
    public class AuthorizationTest
    {
        [TestMethod]
        public async Task CreateAuthorizationHeader()
        {
            var signer = new AwsSigner();
            var config = Helpers.CreateExampleConfig();
            var request = Helpers.CreateExampleRequest();
            var dateTime = DateTimeOffset.Parse("2015-08-30T12:36:00Z");
            var signedHeaders = signer.BuildSignedHeaders(signer.BuildCanonicalHeaders(request));

            const string expected = "Credential=AKIDEXAMPLE/20150830/us-east-1/iam/aws4_request, SignedHeaders=content-type;host;x-amz-date, Signature=5d672d79c15b13162d9279b0855cfba6789a8edb4c82c400e06b5924a6f2b5d7";
            var signature = await signer.CreateSignature(config, request, dateTime);
            var result = signer.CreateAuthorizationHeader(config.AccessId, dateTime, config.Region, config.Service, signedHeaders, signature);
            Assert.AreEqual(expected, result, "Authorization string creation failed");
        }

        [TestMethod]
        public async Task SignRequest()
        {
            var signer = new AwsSigner();
            var config = Helpers.CreateExampleConfig();
            var expectedRequest = Helpers.CreateExampleRequest();
            var request = Helpers.CreateExampleRequest();
            //Remove the X-Amz-Date header since the authorize method should do this
            request.Headers.Remove("X-Amz-Date");

            var expectedDateTime = DateTimeOffset.Parse("2015-08-30T12:36:00Z");
            await AwsSigner.SignRequest(config, request, expectedDateTime);
            var signature = await signer.CreateSignature(config, expectedRequest, expectedDateTime);
            var expectedScheme = "AWS4-HMAC-SHA256";
            var expectedContent = $"Credential=AKIDEXAMPLE/{expectedDateTime:yyyyMMdd}/us-east-1/iam/aws4_request, SignedHeaders=content-type;host;x-amz-date, Signature={signature}";
            Assert.AreEqual(expectedScheme, request.Headers.Authorization.Scheme, "Authorization scheme does not match");
            Assert.AreEqual(expectedContent, request.Headers.Authorization.Parameter, "Authorization content does not match");
        }

        [TestMethod]
        public async Task SignRequestEmail()
        {
            var config = new AwsConfig
            {
                Region = "eu-west-1",
                Service = "ses",
                AccessId = "AKIDEXAMPLE",
                Secret = "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY"
            };
            var request = Helpers.CreateExampleRequest();
            request.Content = new StringContent(string.Empty, Encoding.ASCII, "application/x-www-form-urlencoded");
            //Remove the X-Amz-Date header since the authorize method should do this
            request.Headers.Remove("X-Amz-Date");

            var expectedDateTime = DateTimeOffset.Parse("2018-06-12T20:58:15Z");
            await AwsSigner.SignRequest(config, request, expectedDateTime);
            var expectedScheme = "AWS4-HMAC-SHA256";
            const string expectedContent = "Credential=AKIDEXAMPLE/20180612/eu-west-1/ses/aws4_request, SignedHeaders=content-type;host;x-amz-date, Signature=8675e236d7864e374cb7d8986204275cb191680357c7b1897103a9c4e2547b2c";
            Assert.AreEqual(expectedScheme, request.Headers.Authorization.Scheme, "Authorization scheme does not match");
            Assert.AreEqual(expectedContent, request.Headers.Authorization.Parameter, "Authorization content does not match");
        }
    }
}
