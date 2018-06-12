using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwsSigning;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// This class builds up everything from:
    /// https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
    /// </summary>
    [TestClass]
    public class CanonicalBuilder
    {
        [TestMethod]
        public void CanonicalRequestHasherTest()
        {
            var signer = new AwsSigner();

            var inputBuilder = new StringBuilder();
            inputBuilder.Append("GET\n");
            inputBuilder.Append("/\n");
            inputBuilder.Append("Action=ListUsers&Version=2010-05-08\n");
            inputBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            inputBuilder.Append("host:iam.amazonaws.com\n");
            inputBuilder.Append("x-amz-date:20150830T123600Z\n");
            inputBuilder.Append("\n");
            inputBuilder.Append("content-type;host;x-amz-date\n");
            inputBuilder.Append("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");

            var output = signer.HashText(inputBuilder.ToString());
            const string expected = "f536975d06c0309214f805bb90ccff089219ecd68b2577efef23edd43b7e1a59";
            Assert.AreEqual(expected, output, "Hash content failed");
        }

        [TestMethod]
        public async Task CanonicalRequestCreatedHasherTest()
        {
            var signer = new AwsSigner();
            var request = Helpers.CreateExampleRequest();

            var inputBuilder = new StringBuilder();
            inputBuilder.Append("GET\n");
            inputBuilder.Append("/\n");
            inputBuilder.Append("Action=ListUsers&Version=2010-05-08\n");
            inputBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            inputBuilder.Append("host:iam.amazonaws.com\n");
            inputBuilder.Append("x-amz-date:20150830T123600Z\n");
            inputBuilder.Append("\n");
            inputBuilder.Append("content-type;host;x-amz-date\n");
            inputBuilder.Append("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");

            var canonicalRequest = await signer.BuildCanonicalRequest(request);
            Assert.AreEqual(inputBuilder.ToString(), canonicalRequest, "Request are not equal");

            var output = signer.HashText(canonicalRequest);
            const string expected = "f536975d06c0309214f805bb90ccff089219ecd68b2577efef23edd43b7e1a59";
            Assert.AreEqual(expected, output, "Hash content failed");
        }

        [TestMethod]
        public async Task CanonicalRequestBuilderTest()
        {
            var signer = new AwsSigner();
            var request = Helpers.CreateExampleRequest();
            request.Headers.Add("DifficultContent", "This & That  +  Something = the rest");

            var canonicalRequest = await signer.BuildCanonicalRequest(request);

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("GET\n");
            expectedBuilder.Append("/\n");
            expectedBuilder.Append("Action=ListUsers&Version=2010-05-08\n");
            expectedBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            // The names should be lower cased and then ordered so DifficultContent should be second
            expectedBuilder.Append("difficultcontent:This & That + Something = the rest\n");
            expectedBuilder.Append("host:iam.amazonaws.com\n");
            expectedBuilder.Append("x-amz-date:20150830T123600Z\n");
            expectedBuilder.Append("\n");
            expectedBuilder.Append("content-type;difficultcontent;host;x-amz-date\n");
            expectedBuilder.Append("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
            var expected = expectedBuilder.ToString();
            Assert.AreEqual(expected, canonicalRequest);
        }

        [TestMethod]
        public void PayloadHashTest()
        {
            var signer = new AwsSigner();

            var inputBuilder = new StringBuilder();
            inputBuilder.Append("");

            const string expected = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

            var output = signer.HashText(inputBuilder.ToString());
            Assert.AreEqual(expected, output, "Payload hash failed");
        }

        [TestMethod]
        public async Task PayloadHashContentTest()
        {
            var signer = new AwsSigner();

            var content = new StringContent("Hello World!", Encoding.UTF8, "text/plain");

            const string expected = "7f83b1657ff1fc53b92dc18148a1d65dfc2d4b1fa3d677284addd200126d9069";

            var output = await signer.HashPayload(content);
            Assert.AreEqual(expected, output, "Payload hash failed");
        }

        [TestMethod]
        public void SignedHeadersBuilderTest()
        {
            var signer = new AwsSigner();

            var inputBuilder = new StringBuilder();
            inputBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            inputBuilder.Append("host:iam.amazonaws.com\n");
            inputBuilder.Append("my-header1:a b c\n");
            inputBuilder.Append("my-header1:\"a b c\"\n");
            inputBuilder.Append("x-amz-date:20150830T123600Z\n");

            var expected = "content-type;host;my-header1;my-header1;x-amz-date";

            var output = signer.BuildSignedHeaders(inputBuilder.ToString());
            Assert.AreEqual(expected, output, "Signed headers builder failed");
        }

        [TestMethod]
        public void CanonicalHeadersTest()
        {
            var signer = new AwsSigner();
            var inputHeaders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded; charset=utf-8"),
                new KeyValuePair<string, string>("Host", "iam.amazonaws.com"),
                new KeyValuePair<string, string>("X-AMZ-Date", "20150830T123600Z"),
                new KeyValuePair<string, string>("My-header1", "a  b  c"),
                new KeyValuePair<string, string>("My-header1", "\"a  b  c\"")
            };

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            expectedBuilder.Append("host:iam.amazonaws.com\n");
            expectedBuilder.Append("my-header1:a b c\n");
            expectedBuilder.Append("my-header1:\"a b c\"\n");
            expectedBuilder.Append("x-amz-date:20150830T123600Z\n");
            var expected = expectedBuilder.ToString();

            var output = signer.BuildCanonicalHeaders(inputHeaders);
            Assert.AreEqual(expected, output, "Header builder failed");
        }

        [TestMethod]
        public void CanonicalHeadersOrdinalCaseOrderedTest()
        {
            var signer = new AwsSigner();
            var inputHeaders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded; charset=utf-8"),
                new KeyValuePair<string, string>("host", "iam.amazonaws.com"),
                new KeyValuePair<string, string>("X-AMZ-Date", "20150830T123600Z"),
                new KeyValuePair<string, string>("my-header1", "a  b  c"),
                new KeyValuePair<string, string>("My-header1", "\"a  b  c\"")
            };

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            expectedBuilder.Append("my-header1:\"a b c\"\n");
            expectedBuilder.Append("x-amz-date:20150830T123600Z\n");
            expectedBuilder.Append("host:iam.amazonaws.com\n");
            expectedBuilder.Append("my-header1:a b c\n");
            var expected = expectedBuilder.ToString();

            var output = signer.BuildCanonicalHeaders(inputHeaders);
            Assert.AreEqual(expected, output, "Header builder failed");
        }

        [TestMethod]
        public void CanonicalHeadersFromContentTest()
        {
            var signer = new AwsSigner();
            var request = Helpers.CreateExampleRequest();
            request.Headers.Add("My-header1", "a  b  c");
            request.Headers.Add("My-header1", "\"a  b  c\"");

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("content-type:application/x-www-form-urlencoded; charset=utf-8\n");
            expectedBuilder.Append("host:iam.amazonaws.com\n");
            expectedBuilder.Append("my-header1:a b c\n");
            expectedBuilder.Append("my-header1:\"a b c\"\n");
            expectedBuilder.Append("x-amz-date:20150830T123600Z\n");
            var expected = expectedBuilder.ToString();

            var output = signer.BuildCanonicalHeaders(request);
            Assert.AreEqual(expected, output, "Header builder failed");
        }

        [TestMethod]
        public void CanonicalHeadersFromNullContentTest()
        {
            var signer = new AwsSigner();
            var request = Helpers.CreateExampleRequest();
            request.Content = null;
            request.Headers.Add("My-header1", "a  b  c");
            request.Headers.Add("My-header1", "\"a  b  c\"");

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append("host:iam.amazonaws.com\n");
            expectedBuilder.Append("my-header1:a b c\n");
            expectedBuilder.Append("my-header1:\"a b c\"\n");
            expectedBuilder.Append("x-amz-date:20150830T123600Z\n");
            var expected = expectedBuilder.ToString();

            var output = signer.BuildCanonicalHeaders(request);
            Assert.AreEqual(expected, output, "Header builder failed");
        }

        [TestMethod]
        public void CanonicalUriBuilderTest()
        {
            var signer = new AwsSigner();
            var request = Helpers.CreateExampleRequest();
            const string expected = "/api/documents%20and%20settings";

            request.RequestUri = new Uri("https://myhost.com/api/documents and settings?query=path");

            var output = signer.BuildCanonicalUri(request);
            Assert.AreEqual(expected, output, "Uri builder failed");
        }

        [TestMethod]
        public void CanonicalQueryStringBuilderTest()
        {
            const string input = "Version=2010-05-08&Action=ListUsers";
            const string expected = "Action=ListUsers&Version=2010-05-08";

            var signer = new AwsSigner();
            var output = signer.BuildCanonicalQueryString(input);

            Assert.AreEqual(expected, output, "Query String builder failed");
        }

        [TestMethod]
        public void CanonicalQueryStringBuilderOrdinalTest()
        {
            const string input = "Version=2010-05-08&Action=ListUsers&bA=ab";
            const string expected = "Action=ListUsers&Version=2010-05-08&bA=ab";

            var signer = new AwsSigner();
            var output = signer.BuildCanonicalQueryString(input);

            Assert.AreEqual(expected, output, "Query String builder failed");
        }

        [TestMethod]
        public void CanonicalQueryStringBuilderEncodingTest()
        {
            const string input = "Email=thomas.bruggink@infosupport.com&Version=2010-05-08&Action=ListUsers&bA=ab";
            const string expected = "Action=ListUsers&Email=thomas.bruggink%40infosupport.com&Version=2010-05-08&bA=ab";

            var signer = new AwsSigner();
            var output = signer.BuildCanonicalQueryString(input);

            Assert.AreEqual(expected, output, "Query String builder failed");
        }
    }
}
