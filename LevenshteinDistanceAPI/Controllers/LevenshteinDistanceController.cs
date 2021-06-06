using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevenshteinDistanceAPI.Controllers
{
    [ApiController]
    public class LevenshteinDistanceController : ControllerBase
    {
        [Authorize(Policy = "OAuth2Secure")]
        [HttpGet("getLevenshteinDistance")]
        public IActionResult GetLevenshteinDistance([FromQuery] string value1, string value2)
        {
            if (string.IsNullOrEmpty(value1))
                return BadRequest("value1 is null");
            if (string.IsNullOrEmpty(value2))
                return BadRequest("value2 is null");

            return Ok(Calculate(value1, value2));
        }

        private static int Calculate(string source1, string source2)
        {
            var matrix = new int[source1.Length + 1, source2.Length + 1];

            // Initialization of matrix with row size source1.Length and columns size source2.Length
            for (var i = 0; i <= source1.Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2.Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1.Length; i++)
            {
                for (var j = 1; j <= source2.Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            // return result
            return matrix[source1.Length, source2.Length];
        }
    }
}
