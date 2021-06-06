using LevenshteinDistanceAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace LevenshteinDistanceAPI.Tests.Controllers
{
    public class LevenshteinDistanceControllerTests
    {
        [Fact]
        public void GetLevenshteinDistance_StateUnderTest_ExpectedBehavior_Value1IsNull()
        {
            // Arrange
            var levenshteinDistanceController = new LevenshteinDistanceController();
            string value1 = null;
            string value2 = "value2";

            // Act
            var result = levenshteinDistanceController.GetLevenshteinDistance(
                value1,
                value2);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("value1 is null", ((ObjectResult)result).Value.ToString());
        }

        [Fact]
        public void GetLevenshteinDistance_StateUnderTest_ExpectedBehavior_Value2IsNull()
        {
            // Arrange
            var levenshteinDistanceController = new LevenshteinDistanceController();
            string value1 = "value1";
            string value2 = "";

            // Act
            var result = levenshteinDistanceController.GetLevenshteinDistance(
                value1,
                value2);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("value2 is null", ((ObjectResult)result).Value.ToString());
        }

        [Fact]
        public void GetLevenshteinDistance_StateUnderTest_ExpectedBehavior_expectedBehavior()
        {
            // Arrange
            var levenshteinDistanceController = new LevenshteinDistanceController();
            string value1 = "value1";
            string value2 = "value2";

            // Act
            var result = levenshteinDistanceController.GetLevenshteinDistance(
                value1,
                value2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("1", ((ObjectResult)result).Value.ToString());
        }
    }
}
