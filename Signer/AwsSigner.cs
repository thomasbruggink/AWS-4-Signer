using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AwsSigning
{
    public class AwsConfig
    {
        public string Region { get; set; }
        public string Service { get; set; }
        public string AccessId { get; set; }
        public string Secret { get; set; }
    }

    /// <summary>
    /// This class helps create an AWS signed authorization header
    /// </summary>
    public class AwsSigner : IDisposable
    {
        private readonly SHA256 _sha256;
        private readonly HMAC _hmac;

        public AwsSigner()
        {
            _sha256 = SHA256.Create();
            _hmac = new HMACSHA256();
        }

        /// <summary>
        /// Signs an httprequestmessage with the given config
        /// </summary>
        /// <param name="config"></param>
        /// <param name="request"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static async Task SignRequest(AwsConfig config, HttpRequestMessage request, DateTimeOffset? dateTime = null)
        {
            using (var signer = new AwsSigner())
            {
                if(dateTime == null)
                    dateTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(5);
                const string scheme = "AWS4-HMAC-SHA256";
                request.Headers.Add("X-Amz-Date", dateTime.Value.ToString("yyyyMMddTHHmmssZ"));
                var signedHeaders = signer.BuildSignedHeaders(signer.BuildCanonicalHeaders(request));
                var signature = await signer.CreateSignature(config, request, dateTime.Value);
                var parameter = signer.CreateAuthorizationHeader(config.AccessId, dateTime.Value, config.Region, config.Service, signedHeaders, signature);
                request.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
            }
        }

        /// <summary>
        /// Create the final authorization header
        /// </summary>
        /// <param name="accessId"></param>
        /// <param name="dateTime"></param>
        /// <param name="region"></param>
        /// <param name="service"></param>
        /// <param name="signedHeaders"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public string CreateAuthorizationHeader(string accessId, DateTimeOffset dateTime, string region, string service, string signedHeaders, string signature)
        {
            return $"Credential={accessId}/{dateTime:yyyyMMdd}/{region}/{service}/aws4_request, SignedHeaders={signedHeaders}, Signature={signature}";
        }

        /// <summary>
        /// Helper class to create a signature using the config and request
        /// </summary>
        /// <param name="config"></param>
        /// <param name="request"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<string> CreateSignature(AwsConfig config, HttpRequestMessage request, DateTimeOffset dateTime)
        {
            var signString = await CreateSignString(config.Region, config.Service, request, dateTime);
            var signingKey = CreateSigningKey(config.Secret, dateTime, config.Region, config.Service);
            var signature = CreateSignature(signingKey, signString);
            return signature;
        }

        /// <summary>
        /// Create the signature
        ///  - https://docs.aws.amazon.com/general/latest/gr/sigv4-calculate-signature.html
        /// </summary>
        /// <param name="signingKey"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CreateSignature(byte[] signingKey, string input)
        {
            return ToHexString(Hmac(signingKey, input));
        }

        /// <summary>
        /// Create the signing key
        ///  - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-string-to-sign.html
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="date"></param>
        /// <param name="region"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public byte[] CreateSigningKey(string secret, DateTimeOffset date, string region, string service)
        {
            var result = Encoding.UTF8.GetBytes($"AWS4{secret}");
            result = Hmac(result, date.ToString("yyyyMMdd"));
            result = Hmac(result, region);
            result = Hmac(result, service);
            result = Hmac(result, "aws4_request");
            return result;
        }

        /// <summary>
        /// Build the signing string
        /// - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-string-to-sign.html
        /// </summary>
        /// <param name="region"></param>
        /// <param name="service"></param>
        /// <param name="request"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<string> CreateSignString(string region, string service, HttpRequestMessage request, DateTimeOffset dateTime)
        {
            var builder = new StringBuilder();
            builder.Append("AWS4-HMAC-SHA256\n");
            builder.Append($"{dateTime:yyyyMMddTHHmmssZ}\n");
            builder.Append($"{dateTime:yyyyMMdd}/{region}/{service}/aws4_request\n");

            var canonicalRequest = await BuildCanonicalRequest(request);
            builder.Append(HashText(canonicalRequest));
            return builder.ToString();
        }

        /// <summary>
        /// Build the canonical request
        /// - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> BuildCanonicalRequest(HttpRequestMessage request)
        {
            var builder = new StringBuilder();
            // Part 1
            builder.Append(request.Method + "\n");
            // Part 2
            builder.Append(BuildCanonicalUri(request) + "\n");
            // Part 3
            builder.Append(BuildCanonicalQueryString(request.RequestUri.Query) + "\n");
            var canonicalHeaders = BuildCanonicalHeaders(request);
            // Part 4
            builder.Append(canonicalHeaders);
            builder.Append("\n");
            // Part 5
            builder.Append(BuildSignedHeaders(canonicalHeaders) + "\n");
            // Part 6
            builder.Append(await HashPayload(request.Content));

            // Part 7
            return builder.ToString();
        }
        
        /// <summary>
        /// The payload needs to be sha hashed and hex encoded
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> HashPayload(HttpContent content)
        {
            var payload = string.Empty;
            if (content != null)
                payload = await content.ReadAsStringAsync();
            return HashText(payload);
        }

        /// <summary>
        /// Helper method for hashing text input
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string HashText(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = _sha256.ComputeHash(bytes);
            return ToHexString(hash);
        }

        /// <summary>
        /// Build up the signed header string
        ///  - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        ///  - part 5
        /// </summary>
        /// <param name="canonicalHeaders"></param>
        /// <returns></returns>
        public string BuildSignedHeaders(string canonicalHeaders)
        {
            // Keys should be lower cased but since we use the canonicalheaders output this is already the case
            return string.Join(';', canonicalHeaders.Split('\n').Where(k => !string.IsNullOrWhiteSpace(k)).Select(k => k.Split(':').First().ToLower()));
        }

        /// <summary>
        /// Build the canonical uri
        /// - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        /// - part 2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string BuildCanonicalUri(HttpRequestMessage request)
        {
            // Uri encodes the input correctly by default
            return request.RequestUri.AbsolutePath;
        }

        /// <summary>
        /// Helper class to build the canonical headers with the httprequestmessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string BuildCanonicalHeaders(HttpRequestMessage request)
        {
            var headers = new List<KeyValuePair<string, string>>();
            foreach (var clientDefaultRequestHeader in request.Headers) 
                headers.AddRange(clientDefaultRequestHeader.Value.Select(value => new KeyValuePair<string, string>(clientDefaultRequestHeader.Key, value)));
            headers.Add(new KeyValuePair<string, string>("Host", request.RequestUri.Host));
            if(request.Content != null)
                headers.Add(new KeyValuePair<string, string>("Content-Type", request.Content.Headers.ContentType.ToString()));
            return BuildCanonicalHeaders(headers);
        }

        /// <summary>
        /// Build the canonical headers
        ///  - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        ///  - part 4
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string BuildCanonicalHeaders(List<KeyValuePair<string, string>> headers)
        {
            var spaceRegex = new Regex(" +");
            var builder = new StringBuilder();
            foreach (var header in headers.OrderBy(k => k.Key, StringComparer.Ordinal))
            {
                var value = header.Value;
                value = spaceRegex.Replace(value, " ");
                builder.Append($"{header.Key.ToLower()}:{value}\n");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Build the canonical query string
        /// - https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        /// - part 3
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public string BuildCanonicalQueryString(string queryString)
        {
            var parsedQueryString = HttpUtility.ParseQueryString(queryString);
            var normalizedParams = 
                (from queryParam in parsedQueryString.AllKeys.OrderBy(k => k, StringComparer.Ordinal) 
                    let key = Uri.EscapeDataString(queryParam) 
                    let value = Uri.EscapeDataString(parsedQueryString[queryParam]) 
                        select $"{key}={value}").ToList();

            return string.Join('&', normalizedParams);
        }

        /// <summary>
        /// HMAC helper function
        /// </summary>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private byte[] Hmac(byte[] key, string input)
        {
            _hmac.Key = key;
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = _hmac.ComputeHash(bytes);
            return hash;
        }

        /// <summary>
        /// Helper class to create a hex string from bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ToHexString(byte[] input)
        {
            return input.Aggregate(string.Empty, (current, x) => current + string.Format("{0:x2}", x)).ToLower();
        }

        /// <summary>
        /// Dispose the crypto instances
        /// </summary>
        public void Dispose()
        {
            _sha256?.Dispose();
            _hmac?.Dispose();
        }
    }
}
