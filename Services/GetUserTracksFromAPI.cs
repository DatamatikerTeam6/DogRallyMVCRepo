using DogRallyMVC.Models;
using Newtonsoft.Json;

namespace DogRallyMVC.Services
{
    public class GetUserTracksFromAPI : IGetUserTracksFromAPI
    {
        public async Task<List<TrackDTO>> GetUserTracks(HttpClient client, string userID)
        {
            //Give a user ID 
            var url = $"https://localhost:7183/Tracks/GetUserTracks?userID={userID}"; 

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

        public async Task<List<TrackDTO>> GetAllUserTracks(HttpClient client)
        {
            //Give a user ID 
            var url = $"https://localhost:7183/Tracks/GetAllTracks/"; 

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
