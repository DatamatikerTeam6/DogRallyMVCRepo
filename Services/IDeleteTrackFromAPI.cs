namespace DogRallyMVC.Services
{
    public interface IDeleteTrackFromAPI
    {
        Task DeleteTrack(HttpClient client, int trackID);
    }
}