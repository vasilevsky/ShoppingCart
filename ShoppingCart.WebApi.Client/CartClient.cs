using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingCart.WebApi.Client
{
    public class CartClient
    {
        private readonly CartClientConfiguration configuration;
        private readonly HttpClient httpClient;

        public CartClient(CartClientConfiguration configuration, HttpClient httpClient)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
        }

        public async Task<CartData> GetCart(Guid cartId)
        {
            var request = CreateHttpRequest(HttpMethod.Get, $"/cart/{cartId}");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
                await HandleResponse(response).ConfigureAwait(false);

            return await ReadContentAs<CartData>(response.Content).ConfigureAwait(false);
        }

        public async Task<Guid> CreateCart(ItemData itemData)
        {
            var request = CreateHttpRequest(HttpMethod.Post, "/cart", Serialize(itemData));

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
                await HandleResponse(response).ConfigureAwait(false);

            return await ReadContentAs<Guid>(response.Content).ConfigureAwait(false);
        }

        public async Task AddToCart(Guid cartId, ItemData itemData)
        {
            var request = CreateHttpRequest(HttpMethod.Post, $"/cart/{cartId}", Serialize(itemData));

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.NoContent)
                await HandleResponse(response).ConfigureAwait(false);
        }

        public async Task ClearCart(Guid cartId)
        {
            var request = CreateHttpRequest(HttpMethod.Delete, $"/cart/{cartId}/items");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.NoContent)
                await HandleResponse(response).ConfigureAwait(false);
        }

        public async Task UpdateQuantity(Guid cartId, int productId, int increment)
        {
            var request = CreateHttpRequest(new HttpMethod("Patch"), $"/cart/{cartId}/items");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.NoContent)
                await HandleResponse(response).ConfigureAwait(false);
        }

        public async Task DeleteItems(Guid cartId, int productId)
        {
            var request = CreateHttpRequest(HttpMethod.Delete, $"/cart/{cartId}/items/{productId}");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.NoContent)
                await HandleResponse(response).ConfigureAwait(false);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new CartClientException(
                        "Server replied with Unauthorized. Ensure you are passing correct CartClientConfiguration.ApiKey",
                        response.StatusCode,
                        responseBody);

                case HttpStatusCode.BadRequest:
                    throw new CartClientException("Invalid parameters", response.StatusCode, responseBody);

                case HttpStatusCode.NotFound:
                    throw new CartClientException("Resource not found", response.StatusCode, responseBody);

                default:
                    throw new CartClientException("Unexpected reposponse", response.StatusCode, responseBody);
            }
        }

        private StringContent Serialize(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod method, string path = "", HttpContent content = null)
        {
            var requestMessage = new HttpRequestMessage(method, configuration.EndpointAddress + path);

            if (content != null)
                requestMessage.Content = content;

            SetHeaders(requestMessage);

            return requestMessage;
        }

        private void SetHeaders(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.Accept.Clear();
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", configuration.ApiKey);
        }

        private async Task<T> ReadContentAs<T>(HttpContent httpContent)
        {
            var content = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
