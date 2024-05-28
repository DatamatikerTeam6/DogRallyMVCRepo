using DogRallyMVC.Models;
using DogRallyMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DogRallyMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IUserService userService, IHttpClientFactory httpClientFactory)
        {
            _userService = userService;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            try
            {
                var response = await _userService.RegisterUser(registerDTO, client);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ApiResponse"] = "Din bruger er blevet oprettet.";
                    RedirectToAction("Index", "Home");  // You could also redirect to a success page as needed
                }
                else
                {
                    // Read the response body for error details
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API returned an error: {errorResponse}");

                    TempData["ApiResponse"] = $"API Error: {errorResponse}";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");

                // Returns to the view with errors
                TempData["ApiResponse"] = $"Exception: {ex.Message}";
            }

            // Returns to the view with errors 
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDTO loginDTO)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            try
            {
                var response = await _userService.AuthenticateUser(loginDTO, client);
                if (response.IsSuccessStatusCode)
                {
                    // The token is returned in the response body
                    var token = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("JWTToken", token);

                    //Configure HttpClient with JWT token for subsequent requests
                    ConfigureHttpClientWithToken(client, token);

                    // Get and set user ID
                    var userID = GetUserIDFromToken(token);
                    HttpContext.Session.SetString("UserID", userID);

                    TempData["LoginResponseFromAPI"] = "Du er nu logget ind.";
                    RedirectToAction("Index", "Home");
                }
                else
                {
                    // Read the response body for error details
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API returned an error: {errorResponse}");
                    TempData["LoginResponseFromAPI"] = $"API Error: {errorResponse}";  // Storing error response in ViewBag for display
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");
                TempData["LoginResponseFromAPI"] = $"Exception: {ex.Message}";  // Store exception message in TempData
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });  // Returns to the view with errors and ViewBag information
        }

        private void ConfigureHttpClientWithToken(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        private string GetUserIDFromToken(string token)
        {
            JwtSecurityTokenHandler handler = new();
            var jwtToken = handler.ReadJwtToken(token);
            var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            return userEmail?.Value;
        }


    }
}
