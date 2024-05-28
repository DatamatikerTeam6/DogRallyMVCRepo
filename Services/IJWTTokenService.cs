namespace DogRallyMVC.Services
{
    public interface IJWTTokenService
    {
         bool IsUserAdmin(string token);
    }
}