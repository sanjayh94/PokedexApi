using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PokedexApi.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task Get_NonExistent_Pokemon_Returns_404()
        {
            // Arrange
            string nonExistentPokemon = "idonotexist";
            string url = $"/pokemon/{nonExistentPokemon}";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_NonExistent_PokemonTranslated_Returns_404()
        {
            // Arrange
            string nonExistentPokemon = "idonotexist";
            string url = $"/pokemon/translated/{nonExistentPokemon}";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Pokemon_ReturnsExpectedJsonFormat()
        {
            // Arrange
            string pokemonName = "ditto";
            string url = $"/pokemon/{pokemonName}";

            // Act 
            var response = await _client.GetAsync(url);
            var model = JsonConvert.DeserializeObject<ExpectedPokemonResponse>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.NotNull(model?.Name);
            Assert.NotNull(model?.IsLegendary);
            Assert.NotNull(model?.Description);
            Assert.NotNull(model?.Habitat);
        }

        //[Fact(Skip = "Skipping Translated Endpoint as Public Endpoint is ratelimited to 5 calls per hour")]
        [Fact]
        public async Task Get_PokemonTranslated_ReturnsExpectedJsonFormat()
        {
            // Arrange
            string pokemonName = "ditto";
            string url = $"/pokemon/translated/{pokemonName}";

            // Act 
            var response = await _client.GetAsync(url);
            var model = JsonConvert.DeserializeObject<ExpectedPokemonResponse>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.NotNull(model?.Name);
            Assert.NotNull(model?.IsLegendary);
            Assert.NotNull(model?.Description);
            Assert.NotNull(model?.Habitat);
        }

        [Fact]
        public async Task Get_Pokemon_ReturnsExpectedJsonContent()
        {
            // Arrange
            string pokemonName = "ditto";
            string url = $"/pokemon/{pokemonName}";
            string expectedPokemonResponse = JsonConvert.SerializeObject(new ExpectedPokemonResponse 
            {            
                Name = "ditto",
                Habitat = "urban",
                IsLegendary = false,
                Description = "It can freely recombine its own cellular structure to transform into other life-forms."
            }); 
            
            
            // Act 
            var response = await _client.GetAsync(url);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedPokemonResponse, result);
        }

        //[Fact (Skip ="Skipping Translated Endpoint as Public Endpoint is ratelimited to 5 calls per hour")]
        [Fact]
        public async Task Get_PokemonTranslated_Cave_ReturnsExpectedJsonContent_YodaTranslation()
        {
            // Arrange
            string pokemonName = "onix";
            string url = $"/pokemon/translated/{pokemonName}";
            string expectedPokemonResponse = JsonConvert.SerializeObject(new ExpectedPokemonResponse
            {
                Name = "onix",
                Habitat = "cave",
                IsLegendary = false,
                Description = "As it grows,To become similar to a diamond,  the stone portions of its body harden,But colored black."
            });

            // Act 
            var response = await _client.GetAsync(url);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedPokemonResponse, result);
        }

        //[Fact(Skip = "Skipping Translated Endpoint as Public Endpoint is ratelimited to 5 calls per hour")]
        [Fact]
        public async Task Get_PokemonTranslated_Legendary_ReturnsExpectedJsonContent_YodaTranslation()
        {
            // Arrange
            string pokemonName = "mewtwo";
            string url = $"/pokemon/translated/{pokemonName}";
            string expectedPokemonResponse = JsonConvert.SerializeObject(new ExpectedPokemonResponse
            {
                Name = "mewtwo",
                Habitat = "rare",
                IsLegendary = true,
                Description = "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was."
            });

            // Act 
            var response = await _client.GetAsync(url);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedPokemonResponse, result);
        }

        //[Fact(Skip = "Skipping Translated Endpoint as Public Endpoint is ratelimited to 5 calls per hour")]
        [Fact]
        public async Task Get_PokemonTranslated_ReturnsExpectedJsonContent_ShakespeareTranslation()
        {
            // Arrange
            string pokemonName = "charmander";
            string url = $"/pokemon/translated/{pokemonName}";
            string expectedPokemonResponse = JsonConvert.SerializeObject(new ExpectedPokemonResponse
            {
                Name = "charmander",
                Habitat = "mountain",
                IsLegendary = false,
                Description = "Obviously prefers hot places. At which hour 't rains,  steam is did doth sayeth to spout from the tip of its tail."
            });

            // Act 
            var response = await _client.GetAsync(url);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedPokemonResponse, result);
        }

    }
}
