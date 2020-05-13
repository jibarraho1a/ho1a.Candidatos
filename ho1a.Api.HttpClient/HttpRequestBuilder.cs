using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ho1a.Api.HttpClient
{
    using System.Net.Http;
    public class HttpRequestBuilder
    {
        private string acceptHeader = "application/json";
        private bool allowAutoRedirect;
        private string bearerToken = string.Empty;
        private HttpContent content;
        private HttpMethod method;
        private string requestUri = string.Empty;
        private TimeSpan timeout = new TimeSpan(0, 0, 15);

        private void EnsureArguments()
        {
            if (this.method == null)
            {
                throw new ArgumentNullException("Method");
            }

            if (string.IsNullOrEmpty(this.requestUri))
            {
                throw new ArgumentNullException("Request Uri");
            }
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestBuilder AddAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            this.EnsureArguments();

            // Set up request
            var request = new HttpRequestMessage { Method = this.method, RequestUri = new Uri(this.requestUri) };

            if (this.content != null) request.Content = this.content;

            if (!string.IsNullOrEmpty(this.bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.bearerToken);

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this.acceptHeader))
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.acceptHeader));


            string documentContents;
            if (request.Content != null)
            {
                using (var receiveStream = request.Content.ReadAsStreamAsync()
                    .Result)
                {
                    using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
            }

            var handler = new HttpClientHandler { AllowAutoRedirect = this.allowAutoRedirect };

            var client = new HttpClient(handler) { Timeout = this.timeout };

            return client.SendAsync(request);
        }
    }
}