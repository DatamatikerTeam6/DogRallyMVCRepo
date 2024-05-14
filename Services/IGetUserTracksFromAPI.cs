using DogRallyMVC.Models;
namespace DogRallyMVC.Services
{
    public interface IGetUserTracksFromAPI
    {
        Task<List<TrackDTO>> GetUserTracks(HttpClient client, int userID);
    }
}