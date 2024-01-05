using System.Text;
using System.Text.Json;

namespace COMP_2001_Report.Services
{
    public class AuthService
    {
        private readonly HttpClient? _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<string> AuthenticateAsync(string email, string password)
        {
            //API endpoint
            var apiUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/user";

            var payload = new
            {
                email,
                password
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync(apiUrl, content);

            //successful responce
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }

        }
    }
}
