using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using LevenshteinDistanceUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LevenshteinDistanceUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(StringInputModel input)
        {
            string distance;

            var accessToken = await GetAccessToken();
            distance = await GetLevenshteinDistance(input, accessToken);

            if (!string.IsNullOrEmpty(distance))
                ViewData["Result"] = distance;
            else
                ViewData["Result"] = "Error occurred";

            return View();
        }

        private static async Task<string> GetLevenshteinDistance(StringInputModel input, TokenModel accessToken)
        {
            string distance;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:44339");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.access_token);
                distance = await httpClient.GetStringAsync($"/getLevenshteinDistance?value1={input.Number1}&value2={input.Number2}");
            }

            return distance;
        }

        private async Task<TokenModel> GetAccessToken()
        {
            var tokenRequestData = new Dictionary<string, string>()
            {
                { "client_id", "secret_client_id" },
                { "client_secret", "secret" },
                { "scope", "apiscope" },
                { "grant_type", "client_credentials" }
            };
            string resp;
            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(tokenRequestData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync("https://localhost:44345/connect/token", content);
                    response.EnsureSuccessStatusCode();
                    resp = await response.Content.ReadAsStringAsync();
                }
            }

            return JsonConvert.DeserializeObject<TokenModel>(resp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
