using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokedexApi.Tests
{
    public class PokemonSmokeTests : IClassFixture<WebApplicationFactory<PokedexApi.Startup>>
    {
        private readonly HttpClient _client;

        public PokemonSmokeTests(WebApplicationFactory<PokedexApi.Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task CheckWebStatus_ReturnsSuccess()
        {
            // What it tests:
            // ->  Web API application starts
            // ->  Server is running and can handle requests
            // ->  Required services are registered with the dependency injection container
            // ->  Middleware pipeline is correctly configured
            // ->  Routing sends requests to the expected endpoint

            // Arrange
            string url = "/";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    
    }
}
