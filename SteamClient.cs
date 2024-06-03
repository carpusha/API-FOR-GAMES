using Newtonsoft.Json;

namespace ApiGAMES
{
    public class SteamClient
    {
        private static string _address;
        private static string _apikey;
        private static string _apiHost;

        public SteamClient()
        {
            _address = Constants.Address;
            _apikey = Constants.ApiKey;
            _apiHost = Constants.ApiHost;
        }

        public async Task<Game> GetInformation(string name, string price, int Id, string steamUrl, string imageUrl, string type, string ReleaseDate)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_address),
                Headers =
    {
        { "X-RapidAPI-Key", _apikey },
        { "X-RapidAPI-Host", _apiHost },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Game>(body);

                return result;
            }
        }
    }
}
