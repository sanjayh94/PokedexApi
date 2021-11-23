using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokedexApi.Controllers;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PokedexApi.Tests
{
    public class PokemonControllerUnitTests
    {
        [Fact]
        public async Task GetPokemon_Returns_Correct_TypeAsync()
        {
            // Arrange
            string pokemon = "ditto";
            var fakePokemon = A.Fake<Pokemon>();

            fakePokemon.Name = "ditto";
            fakePokemon.Species.IsLegendary = false;
            fakePokemon.Species.Habitat.Name = "urban";
            fakePokemon.Species.FlavorTextEntries.Add(
                new FlavorTextEntry() {
                    Language= new Info(){ Name="en"}, 
                    FlavorText= "It can freely recombine its own cellular structure to\ntransform into other life-forms."
            });
            fakePokemon.ApiResponseStatus = System.Net.HttpStatusCode.OK;


            var pokemonService = A.Fake<IPokemonService>();
            var translatorService = A.Fake<ITranslatorService>();


            // Create a real logger instance (rather than creating a Mock)
            var serviceProvider = new ServiceCollection()
                                    .AddLogging()
                                    .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<PokemonController>();

            A.CallTo(() => pokemonService.GetPokemon(pokemon)).Returns(Task.FromResult(fakePokemon));

            var controller = new PokemonController(pokemonService, translatorService, logger);

            // Act
            var actionResult = await controller.GetPokemon(pokemon);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var returnPokemon = result.Value as PokemonDTO;
            Assert.IsType<PokemonDTO>(returnPokemon);
        }
    }
}
