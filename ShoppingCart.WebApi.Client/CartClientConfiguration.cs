using System;

namespace ShoppingCart.WebApi.Client
{
    public class CartClientConfiguration
    {
        public string EndpointAddress { get; private set; }

        public string ApiKey { get; private set; }

        public CartClientConfiguration SetApiKey(string apiKey)
        {
            if (apiKey == null)
                throw new ArgumentNullException(nameof(apiKey));

            ApiKey = apiKey;

            return this;
        }

        public CartClientConfiguration SetEndpointAddress(string endpointAddress)
        {
            if (endpointAddress == null)
                throw new ArgumentNullException(nameof(endpointAddress));

            var address = endpointAddress.TrimEnd('/');
            EndpointAddress = address;

            return this;
        }
    }
}