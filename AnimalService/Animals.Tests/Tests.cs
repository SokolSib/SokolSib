using Animals.DTM.Model;
using Animals.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Animals.Tests
{
    public class Tests
    {
        private readonly HttpClient _client;

        private AnimalType testAnimal = new AnimalType(30, "Миша", "белый медведь", "Север", "Северный полюс", "Медведь", "Белый", string.Empty);

        public Tests()
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(configurationBuilder.Build())
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("Get")]
        public async Task GetAll(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/Animals");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("Get", 1)]
        public async Task GetFirst(string method, int id)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/Animals/{id}");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("Get", "Пятнистый", "Леопард", "Саванна")]
        public async Task Find(string method, string color, string type, params string[] regions)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/Animals/color/{color}/type/{type}/regions/{regions}");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
