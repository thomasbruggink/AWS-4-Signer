using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using AwsSigning.Models;
using Newtonsoft.Json;

namespace AwsSigning
{
    public class AwsValidator
    {
        private readonly HttpClient _httpClient;

        public AwsValidator()
        {
            _httpClient = new HttpClient();
        }

        public static async Task<bool> Verify(AwsConfig config, HttpRequestMessage request)
        {
            var validator = new AwsValidator();
            var type = validator.ParseMessageType(request);
            if (type == MessageType.None)
                return false;
            var content = validator.ParseContent(type, await request.Content.ReadAsStringAsync());
            if(content == null)
                return false;

            var certificate = await validator.DownloadCertificate(new Uri(content.SigningCertURL));
            if (certificate == null)
                return false;
            var publicKey = certificate.GetRSAPublicKey();

            var signString = content.CreateSigningString();
            return publicKey.VerifyData(Encoding.UTF8.GetBytes(signString), Convert.FromBase64String(content.Signature), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        public string BuildSigningString(AwsBase message)
        {
            return message.CreateSigningString();
        }

        private X509Certificate2 ParseCertificate(byte[] certData)
        {
            return new X509Certificate2(certData);
        }

        public async Task<X509Certificate2> DownloadCertificate(Uri endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            var result = await _httpClient.SendAsync(request);
            var content = await result.Content.ReadAsByteArrayAsync();
            return ParseCertificate(content);
        }

        public AwsBase ParseContent(MessageType type, string content)
        {
            switch (type)
            {
                case MessageType.None:
                    return null;
                case MessageType.SubscriptionConfirmation:
                    return JsonConvert.DeserializeObject<AwsSubscriptionConfirmation>(content);
                case MessageType.Notification:
                    return JsonConvert.DeserializeObject<AwsNotification>(content);
                case MessageType.UnsubscribeConfirmation:
                    return JsonConvert.DeserializeObject<AwsUnsubscribeConfirmation>(content);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public MessageType ParseMessageType(HttpRequestMessage request)
        {
            const string messageTypeHeader = "x-amz-sns-message-type";
            if (!request.Headers.Contains(messageTypeHeader))
                return MessageType.None;
            var headers = request.Headers.GetValues(messageTypeHeader).ToList();
            if (headers.Count != 1)
                return MessageType.None;
            return !Enum.TryParse(typeof(MessageType), headers.First(), out var type) ? MessageType.None : (MessageType) type;
        }
    }
}
