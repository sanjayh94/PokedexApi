using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using PokedexApi.Controllers;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PokedexApi.Tests
{
    public class PokemonControllerUnitTests
    {
        #region PrivateVariables
        private readonly Mock<ILogger<PokemonController>> loggerStub = new();
        private readonly Mock<IPokemonService> pokemonServiceStub = new();
        private readonly Mock<ITranslatorService> translatorServiceStub = new();

        //private Pokemon ReturnPokemonObjectWithNecessaryData()
        //{
        //    return new Pokemon()
        //    {
        //        ApiResponseStatus = System.Net.HttpStatusCode.OK,
        //        Name = "ditto",
        //        Species = new PokemonSpecies()
        //        {
        //            Habitat = new Info() { Name = "urban" },
        //            IsLegendary = false,
        //            FlavorTextEntries = new List<FlavorTextEntry>().Add(
        //                new FlavorTextEntry()
        //                {
        //                    FlavorText = "",
        //                    Language = new Info() { Name = "" }
        //                })

        //        }
        //    };

        //}


        #endregion

        [Fact]
        public async Task GetPokemon_NonExistingPokemon_Returns_Not_Found()
        {
            // Arrange           
            var pokemonMock = new Pokemon() { ApiResponseStatus= System.Net.HttpStatusCode.NotFound};
            pokemonServiceStub.Setup(pokemon => pokemon.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonMock);
 
            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

            // Act
            var actionResult = await controller.GetPokemon(It.IsAny<string>());

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetTranslatedPokemon_NonExistingPokemon_Returns_Not_Found()
        {
            // Arrange
            var pokemonTranslatedMock = new PokemonTranslated() { ApiResponseStatus = System.Net.HttpStatusCode.NotFound };
            translatorServiceStub.Setup(pokemon => pokemon.GetTranslatedPokemonDescriptionWithConditions(It.IsAny<string>())).Returns(pokemonTranslatedMock);

            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

            // Act
            var actionResult = await controller.GetTranslatedPokemonDescription(It.IsAny<string>());

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetPokemon_NullObject_Returns_500()
        {
            // Arrange
            pokemonServiceStub.Setup(pokemon => pokemon.GetPokemon(It.IsAny<string>())).ReturnsAsync((Pokemon)null);

            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

            // Act
            var actionResult = await controller.GetPokemon(It.IsAny<string>());

            // Assert            
            var expectedResult = new StatusCodeResult(StatusCodes.Status500InternalServerError);            

            actionResult.Result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetTranslatedPokemon_NullObject_Returns_500()
        {
            // Arrange           
            translatorServiceStub.Setup(pokemon => pokemon.GetTranslatedPokemonDescriptionWithConditions(It.IsAny<string>())).Returns((PokemonTranslated)null);

            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

            // Act
            var actionResult = await controller.GetTranslatedPokemonDescription(It.IsAny<string>());

            // Assert
            var expectedResult = new StatusCodeResult(StatusCodes.Status500InternalServerError);

            actionResult.Result.Should().BeEquivalentTo(expectedResult);
        }

        //[Fact]
        //public async Task GetPokemon_WithExisitingItem_ReturnsExpectedItem()
        //{
        //    //Arrange
            

        //    pokemonServiceStub.Setup(pokemon => pokemon.GetPokemon(It.IsAny<string>())).ReturnsAsync((Pokemon)null);

        //    var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

        //    //Act

        //    //Assert

        //}
    }
}
