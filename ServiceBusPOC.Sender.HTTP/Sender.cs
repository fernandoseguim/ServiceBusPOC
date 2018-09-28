using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusPOC.Sender.HTTP
{
    public class Sender
    {
        public Sender(Uri baseUri, string resouce, SharedAccessSignature sas)
        {
            this.BaseUri = baseUri;
            this.Resouce = resouce;
            this.Sas = sas;
        }

        public Uri BaseUri { get; }
        public string Resouce { get; }
        public SharedAccessSignature Sas { get; }

        public async Task SendMessageAsync (string message)
        {
            var httpClient = new HttpClient { BaseAddress = this.BaseUri };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.Sas.Scheme, this.Sas.Token);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{ this.Resouce }/messages")
                          {
                              Content = new StringContent(message, Encoding.UTF8, "application/atom+xml")
                          };

            await httpClient.SendAsync(request);
        }
    }
}
