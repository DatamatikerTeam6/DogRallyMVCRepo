using DogRallyMVC.Models;
using DogRallyMVC.Services;
using Microsoft.AspNetCore.Mvc;

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
                    TempData["ApiResponse"] = $"API Error: {errorResponse}";  // Storing error response in ViewBag for display
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");
                ViewBag.ApiResponse = $"Exception: {ex.Message}";  // Store exception message in ViewBag
            }

            return RedirectToPage("/Account/Register", new { area = "Identity" });  // Returns to the view with errors and ViewBag information
        }
    }
}
