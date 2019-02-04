using System;
using System.Net;

namespace ShoppingCart.WebApi.Client
{
    public class CartClientException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public string Response { get; private set; }

        public CartClientException(string message, HttpStatusCode statusCode, string response)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length))
        {
            StatusCode = statusCode;
            Response = response;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }
}