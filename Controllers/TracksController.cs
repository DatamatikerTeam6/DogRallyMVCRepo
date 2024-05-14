using DogRallyMVC.Models;
using DogRallyMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace DogRallyMVC.Controllers
{
    public class TracksController : Controller
    {
        // Dependency Injection
        private readonly ILogger<TracksController> _logger;
        private readonly IPostTrackToAPI _postTrackToAPI;
        private readonly IGetExercisesFromAPI _getExercisesFromAPI;
        private readonly IGetTrackFromAPI _getTrackFromAPI;
        private readonly IGetUserTracksFromAPI _getUserTracksFromAPI;
        private readonly IDeleteTrackFromAPI _deleteTrackFromAPI;
        private readonly IHttpClientFactory _httpClientFactory;

        public TracksController(ILogger<TracksController> logger, IPostTrackToAPI postTrackToAPI, IHttpClientFactory httpClientFactory, IGetExercisesFromAPI getExercisesFromAPI, IGetTrackFromAPI getTrackFromAPI, IGetUserTracksFromAPI getUserTracksFromAPI, IDeleteTrackFromAPI deleteTrackFromAPI)
        {
            _logger = logger;
            _postTrackToAPI = postTrackToAPI;
            _httpClientFactory = httpClientFactory;
            _getExercisesFromAPI = getExercisesFromAPI;
            _getTrackFromAPI = getTrackFromAPI;
            _getUserTracksFromAPI = getUserTracksFromAPI;
            _deleteTrackFromAPI = deleteTrackFromAPI;
        }

        public async Task<IActionResult> Index(int id)
        {

            var client = _httpClientFactory.CreateClient();
            var trackDTOs = await _getUserTracksFromAPI.GetUserTracks(client, id);

            if (trackDTOs != null)
                return View(trackDTOs);

            else
                return BadRequest();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrack()
        {
            var client = _httpClientFactory.CreateClient();

            Console.WriteLine("Før kald til GetExercises");

            //Get exercises from API
            var exercises = await _getExercisesFromAPI.GetExercises(client);

            Console.WriteLine("Efter kald til GetExercises");

            Console.WriteLine("Berit");
            var viewModel = new TrackExerciseViewModelDTO
            {
                Track = new TrackDTO(),
                Exercises = exercises
            };
            return View(viewModel);
        }

        [HttpPost]
       
        public async Task<IActionResult> CreateTrack([Bind("Exercises, Track")] TrackExerciseViewModelDTO tevm)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            try
            {
                var response = await _postTrackToAPI.PostTrack(tevm, client);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.SuccessMessage = "Track created successfully!";
                    return RedirectToAction("Index");  // You could also redirect to a success page as needed
                }
                else
                {
                    // Read the response body for error details
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API returned an error: {errorResponse}");
                    ViewBag.ApiResponse = $"API Error: {errorResponse}";  // Storing error response in ViewBag for display
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");
                ViewBag.ApiResponse = $"Exception: {ex.Message}";  // Store exception message in ViewBag
            }

            return View(tevm); // Returns to the view with errors and ViewBag information
        }

        [HttpGet]
        public async Task<IActionResult> ReadTrack(int id)
        {
            var client = _httpClientFactory.CreateClient();

            //Get Track from API
            List<TrackExerciseDTO> trackExercises = await _getTrackFromAPI.GetTrack(client, id);

            foreach (var trackExerciseDTO in trackExercises)
            {
                Console.WriteLine((trackExerciseDTO.ExerciseIllustrationPath).ToString());
            }

            foreach (var trackExerciseDTO in trackExercises)
            {
                _logger?.LogInformation((trackExerciseDTO.ExerciseIllustrationPath).ToString());
            }

            return View(trackExercises);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTrack(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // Get Track from API
            List<TrackExerciseDTO> trackExercises = await _getTrackFromAPI.GetTrack(client, id);

            // Assuming the API returns all exercises for a specific track
            // and assuming TrackDTO and ExerciseDTO can be constructed from TrackExerciseDTO
            var trackDTO = new TrackDTO
            {
                TrackID = trackExercises.FirstOrDefault()?.ForeignTrackID ?? 0,
                TrackName = trackExercises.FirstOrDefault()?.TrackName
            };

            var exerciseDTOs = trackExercises.Select(te => new ExerciseDTO
            {
                ExerciseID = te.ForeignExerciseID,
                ExerciseIllustrationPath = te.ExerciseIllustrationPath,
                ExercisePositionX = te.TrackExercisePositionX,
                ExercisePositionY = te.TrackExercisePositionY
            }).ToList();

            TrackExerciseViewModelDTO viewModel = new TrackExerciseViewModelDTO
            {
                Track = trackDTO,
                Exercises = exerciseDTOs
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrack([Bind("Exercises, Track")] TrackExerciseViewModelDTO tevm)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string json = JsonConvert.SerializeObject(tevm);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _logger.LogInformation(json);

            try
            {
                var response = await client.PutAsync("https://localhost:7183/Tracks/UpdateTrack", content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.SuccessMessage = "Track created successfully!";
                    return RedirectToAction("Index");  // You could also redirect to a success page as needed
                }
                else
                {
                    // Read the response body for error details
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API returned an error: {errorResponse}");
                    ViewBag.ApiResponse = $"API Error: {errorResponse}";  // Storing error response in ViewBag for display
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");
                ViewBag.ApiResponse = $"Exception: {ex.Message}";  // Store exception message in ViewBag
            }
            return View(tevm); // Return to the view with errors and ViewBag information
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                await _deleteTrackFromAPI.DeleteTrack(client, id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}











