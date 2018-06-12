using System;
using System.Net.Http;

namespace AwsSigning
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new AwsConfig
            {
                Region = "eu-west-1",
                Service = "ses",
                AccessId = "<ACCESSID>",
                Secret = "<SECRET>"
            };

            var from = "from@email.com";
            var to = "to@email.com";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                // Optional content
                //Content = new StringContent(string.Empty, Encoding.UTF8, "application/x-www-form-urlencoded"),
                RequestUri = new Uri($"https://email.eu-west-1.amazonaws.com?Action=SendEmail&Source={from}&Destination.ToAddresses.member.1={to}&Message.Subject.Data=AmazonMail&Message.Body.Text.Data=SigningTest&ConfigurationSetName=ExtraInfo")
            };
            AwsSigner.SignRequest(config, request).GetAwaiter().GetResult();

            var client = new HttpClient();
            var result = client.SendAsync(request).GetAwaiter().GetResult();
            result.EnsureSuccessStatusCode();
            Console.WriteLine("All good");
        }
    }
}
