using System.Net.Http;
using System.Threading.Tasks;

namespace ho1a.Api.HttpClient
{
    public static class HttpRequestFactory
    {
        public static Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return DeleteAsync(requestUri, string.Empty);
        }

        public static Task<HttpResponseMessage> DeleteAsync(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder().AddMethod(HttpMethod.Delete)
                ?.AddRequestUri(requestUri)
                ?.AddBearerToken(bearerToken);

            return builder.SendAsync();
        }

        public static Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return GetAsync(requestUri, string.Empty);
        }

        public static Task<HttpResponseMessage> GetAsync(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder().AddMethod(HttpMethod.Get)
                ?.AddRequestUri(requestUri)
                ?.AddBearerToken(bearerToken);

            return builder?.SendAsync();
        }

        public static Task<HttpResponseMessage> PatchAsync(string requestUri, object value)
        {
            return PatchAsync(requestUri, value, string.Empty);
        }

        public static Task<HttpResponseMessage> PatchAsync(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder().AddMethod(new HttpMethod("PATCH"))
                ?.AddRequestUri(requestUri)
                ?.AddContent(new PatchContent(value))
                ?.AddBearerToken(bearerToken);

            return builder?.SendAsync();
        }

        public static Task<HttpResponseMessage> PostAsync(string requestUri, object value)
        {
            return PostAsync(requestUri, value, string.Empty);
        }

        public static Task<HttpResponseMessage> PostAsync(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder().AddMethod(HttpMethod.Post)
                ?.AddRequestUri(requestUri)
                ?.AddContent(new JsonContent(value))
                ?.AddBearerToken(bearerToken);

            return builder?.SendAsync();
        }

        public static Task<HttpResponseMessage> PostFileAsync(string requestUri, string filePath, string apiParamName) => PostFileAsync(requestUri, filePath, apiParamName, string.Empty);

        public static Task<HttpResponseMessage> PostFileAsync(
            string requestUri,
            string filePath,
            string apiParamName,
            string bearerToken)
        {
            var builder = new HttpRequestBuilder().AddMethod(HttpMethod.Post)
                ?.AddRequestUri(requestUri)
                ?.AddContent(new FileContent(filePath, apiParamName))
                ?.AddBearerToken(bearerToken);

            return builder.SendAsync();
        }

        public static Task<HttpResponseMessage> PutAsync(string requestUri, object value) => PutAsync(requestUri, value, string.Empty);

        public static Task<HttpResponseMessage> PutAsync(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder().AddMethod(HttpMethod.Put)
                ?.AddRequestUri(requestUri)
                ?.AddContent(new JsonContent(value))
                ?.AddBearerToken(bearerToken);

            return builder?.SendAsync();
        }
    }
}
