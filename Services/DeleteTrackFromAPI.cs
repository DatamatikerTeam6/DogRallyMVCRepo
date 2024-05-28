using DogRallyMVC.Models;
using Newtonsoft.Json;

namespace DogRallyMVC.Services
{
    public class DeleteTrackFromAPI : IDeleteTrackFromAPI
    {
        public async Task DeleteTrack(HttpClient client, int id)
        {
            var url = $"https://localhost:7183/Tracks/DeleteTrack/{id}"; 

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Failed to delete track: {response.StatusCode} - {errorResponse}");
                }
            }
            catch (Exception ex)
            {               
                throw new Exception("An error occurred while deleting the track.", ex);
            }
        }
    }
}
