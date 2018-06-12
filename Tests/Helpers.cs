using System;
using System.Net.Http;
using System.Text;
using AwsSigning;

namespace Tests
{
    class Helpers
    {
        public static AwsConfig CreateExampleConfig()
        {
            return new AwsConfig
            {
                Region = "us-east-1",
                Service = "iam",
                AccessId = "AKIDEXAMPLE",
                Secret = "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY"
            };
        }

        public static HttpRequestMessage CreateExampleRequest()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://iam.amazonaws.com/?Action=ListUsers&Version=2010-05-08"),
                Content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            // The test date
            request.Headers.Add("X-Amz-Date", "20150830T123600Z");
            return request;
        }
    }
}
