using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToClient(PlatformReadModel platform)
        {
            var httpContent = new StringContent(
                    JsonSerializer.Serialize(platform),
                    Encoding.UTF8,
                    "application/json");

            var response = await _httpClient.PostAsync(
                    _configuration["CommandService"],
                    httpContent);

            if(response.IsSuccessStatusCode)
                Console.WriteLine("====>>>>> Sync POST to Commands Service was successful");
            else
                Console.WriteLine("=====>>>>>> Sync POST to Commands Service failed.");
        }
    }
}