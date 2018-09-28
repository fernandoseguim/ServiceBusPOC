using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ServiceBusPOC.Sender.HTTP
{
    public class SharedAccessSignature
    {
        public SharedAccessSignature(string resouceUri, string policeName, string key)
        {
            this.ResouceUri = resouceUri;
            this.PoliceName = policeName;
            this.Key = key;
        }

        public string ResouceUri { get; }
        public string PoliceName { get; }
        public string Key { get; }

        public int ExpirationTime { get; private set; } = 3600;

        public string Scheme => "SharedAccessSignature";
        public string Token => this.CreateToken();

        private string CreateToken()
        {
            var sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + this.ExpirationTime);
            var stringToSign = HttpUtility.UrlEncode(this.ResouceUri) + "\n" + expiry;
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(this.Key));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = string.Format(CultureInfo.InvariantCulture,
                                         "sr={0}&sig={1}&se={2}&skn={3}",
                                         HttpUtility.UrlEncode(this.ResouceUri), HttpUtility.UrlEncode(signature), expiry, this.PoliceName);

            return sasToken;
        }
    }
}
