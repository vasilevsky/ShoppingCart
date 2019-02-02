using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShoppingCart.WebApi
{
    public static class CustomAuthenticationMiddlewareExtension
    {
        public static void UseCustomAuthenticationMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<CustomAuthenticationMiddleware>();
        }
    }

    public class CustomAuthenticationMiddleware
    {
        private const string AuthorizationHeaderKey = "Authorization";
        private const string Scheme = "ApiKey";
        private readonly RequestDelegate _next;
        private readonly IAuthenticationService authenticationService;

        public CustomAuthenticationMiddleware(RequestDelegate next, IAuthenticationService authorizationService)
        {
            _next = next;
            this.authenticationService = authorizationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(AuthorizationHeaderKey, out StringValues value))
            {
                if (!StringValues.IsNullOrEmpty(value)
                    && AuthenticationHeaderValue.TryParse(value, out AuthenticationHeaderValue authValue))
                {
                    if (authValue.Scheme == Scheme
                        && authenticationService.ValidateKey(authValue.Parameter))
                    {
                        await _next(context);
                    }
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }
    }
}
