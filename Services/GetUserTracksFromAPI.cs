using DogRallyMVC.Models;
using Newtonsoft.Json;

namespace DogRallyMVC.Services
{
    public class GetUserTracksFromAPI : IGetUserTracksFromAPI
    {
        public async Task<List<TrackDTO>> GetUserTracks(HttpClient client, int userID)
        { 
            var url = $"https://localhost:7183/Tracks/GetUserTracks"; //Give a user ID here

            try
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var exercises = JsonConvert.DeserializeObject<List<TrackDTO>>(responseBody);
                    return exercises;
                }
                else
                {
                    Console.WriteLine($"Anmodningen mislykkedes med statuskode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Der opstod en undtagelse: {ex.Message}");
            }
            return null;
        }
    }
}
