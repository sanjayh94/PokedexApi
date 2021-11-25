using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PokedexApi.Controllers;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PokedexApi.Tests
{
    /// <summary>
    /// Class with methods to Unit test PokemonController. Uses XUnit for Unit testing, Moq library for Mocks and FluentAssertions for assertions.
    /// </summary>
    public class PokemonControllerUnitTests
    {
        #region PrivateVariables
        private readonly Mock<ILogger<PokemonController>> loggerStub = new(); // Mock Stub of logger dependency
        private readonly Mock<IPokemonService> pokemonServiceStub = new(); // Mock Stub of pokemonService dependency
        private readonly Mock<ITranslatorService> translatorServiceStub = new(); // Mock Stub of translatorService dependency

        private static Pokemon GetMockPokemonObjectWithNecessaryData(string name, string habitat, bool isLegendary, string description, HttpStatusCode apiResponse = HttpStatusCode.OK)
        {
            return new Pokemon()
            {
                ApiResponseStatus = apiResponse,
                Name = name,
                Species = new PokemonSpecies()
                {
                    Habitat = new Info() { Name = habitat },
                    IsLegendary = isLegendary,
                    FlavorTextEntries = new List<FlavorTextEntry>
                        {
                            new FlavorTextEntry()
                            {
                                FlavorText = description,
                                Language = new Info() { Name = "en" }
                            }
                        }                    

                }
            };

        }

        private static PokemonDTO GetMockPokemonDTOObjectWithNecessaryData(string name, string habitat, bool isLegendary, string description) 
        {
            return new PokemonDTO()
            {
                Name = name,
                Habitat = habitat,
                IsLegendary = isLegendary,
                Description = description
            };
        }

        private static PokemonTranslated GetMockPokemonTranslatedObjectWithNecessaryData(string name, string habitat, bool isLegendary, string description, HttpStatusCode apiResponse = HttpStatusCode.OK)
        {
            return new PokemonTranslated()
            {
                Name = name,
                Habitat = habitat,
                IsLegendary = isLegendary,
                Description = description,
                ApiResponseStatus = apiResponse
            };
        }
        #endregion

        /// <summary>
        /// Unit test to Get Pokemon method with non existing Pokemon. Should return Not Found
        /// </summary>
        [Fact]
        public async Task GetPokemon_NonExistingPokemon_Returns_Not_Found()
        {
            // Arrange           
            var pokemonMock = new Pokemon() { ApiResponseStatus= System.Net.HttpStatusCode.NotFound}; //Mock of fake Pokemon class
            // Tell pokemonServiceStub to make a call to GetPokemon method of pokemonService using any string as input parameter (since it doesn't matter) and returns the fake mock of Pokemon Class
            pokemonServiceStub.Setup(pokemon => pokemon.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonMock); 

            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object); // Pass Dependency Injection variables when initialising Controller

            // Act
            var actionResult = await controller.GetPokemon(It.IsAny<string>()); // Make a call to GetPokemon method of the controller using any string (Input doesn't matter)

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>(); //Assert the response should be of type NotFoundResult
        }

        /// <summary>
        /// Unit test to Get Pokemon Translated method with non existing Pokemon. Should return Not Found
        /// </summary>
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

        /// <summary>
        /// Unit test to Get Pokemon method with null return from PokemonService. Should return 500 InternalServerError
        /// Used to test graceful failure
        /// </summary>
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

        /// <summary>
        /// Unit test to Get Pokemon Translated method with null return from PokemonService. Should return 500 InternalServerError
        /// Used to test graceful failure
        /// </summary>
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

        /// <summary>
        /// Unit test to Get Pokemon method with an existing pokemon. Should return expected Pokemon result
        /// </summary>
        [Fact]
        public async Task GetPokemon_WithExisitingItem_ReturnsExpectedItem()
        {
            //Arrange
            var expectedPokemon = GetMockPokemonObjectWithNecessaryData(name: "ditto", habitat: "urban", isLegendary: false,
                description: "blah blah blah");
            pokemonServiceStub.Setup(pokemon => pokemon.GetPokemon(It.IsAny<string>())).ReturnsAsync(expectedPokemon);

            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

            //Act
            var actionResult = await controller.GetPokemon(It.IsAny<string>());

            //Assert
            // Initialise an OkObjectResult with the Mock PokemonDTO data as that is what we will be expecting
            var expectedResult = new OkObjectResult(GetMockPokemonDTOObjectWithNecessaryData(name: "ditto", habitat: "urban", isLegendary: false,
                description: "blah blah blah"));

            actionResult.Result.Should().BeEquivalentTo(expectedResult);
        }

        /// <summary>
        /// Unit test to Get Pokemon Translated method with an existing pokemon. Should return expected Pokemon result
        /// </summary>
        [Fact]
        public async Task GetTranslatedPokemon_WithExisitingItem_ReturnsExpectedItem()
        {
            //Arrange
            var expectedPokemonTranslated = GetMockPokemonTranslatedObjectWithNecessaryData(name: "ditto", habitat: "urban", isLegendary: false,
                description: "blah blah blah");
            translatorServiceStub.Setup(pokemon => pokemon.GetTranslatedPokemonDescriptionWithConditions(It.IsAny<string>())).Returns(expectedPokemonTranslated);

            var controller = new PokemonController(pokemonServiceStub.Object, translatorServiceStub.Object, loggerStub.Object);

            //Act
            var actionResult = await controller.GetTranslatedPokemonDescription(It.IsAny<string>());

            //Assert
            // Initialise an OkObjectResult with the Mock PokemonDTO data as that is what we will be expecting
            var expectedResult = new OkObjectResult(GetMockPokemonDTOObjectWithNecessaryData(name: "ditto", habitat: "urban", isLegendary: false,
                description: "blah blah blah"));

            actionResult.Result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
