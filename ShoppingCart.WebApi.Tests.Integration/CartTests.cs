using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Shouldly;
using Xunit;

namespace ShoppingCart.WebApi.Tests.Integration
{
    public class CartTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;

        public CartTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Gets_cart()
        {
            var client = _fixture.CreateClient();
            var cartId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/cart/{cartId}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
