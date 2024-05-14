using DogRallyMVC.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace DogRallyMVC.Services
{
    public class UserService : IUserService
    {
        public async Task<HttpResponseMessage> RegisterUser(RegisterDTO registerDTO, HttpClient httpClient)
        {
            try
            {
                var json = JsonConvert.SerializeObject(registerDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://localhost:7183/api/accounts/register", content);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Det lykkedes ikke at oprette en konto: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error serializing the registration data.", ex);
            }
        }

        public async Task<string> AuthenticateUser(UserDTO userDTO, HttpClient httpClient)
        {
            var response = await httpClient.PostAsJsonAsync("https://localhost:7183/login", userDTO);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            else
            {
                throw new Exception("Autentificering mislykkedes.");
            }
        }
    }
}
