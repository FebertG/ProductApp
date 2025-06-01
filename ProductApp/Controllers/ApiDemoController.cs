using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApp.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductApp.Controllers
{
    /// <summary>
    /// Controller odpowiedzialny za komunikację z zewnętrznym API JSONPlaceholder.
    /// Udostępnia akcje GET i POST, loguje wyniki do konsoli, a także przekazuje je do widoku.
    /// </summary>
    public class ApiDemoController : Controller
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Konstruktor, wstrzykuje HttpClient i ustawia BaseAddress.
        /// </summary>
        public ApiDemoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            // Ustawiamy bazowy adres dla wszystkich zapytań HTTP
            _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        }

        /// <summary>
        /// GET: /ApiDemo
        /// Strona startowa z opcjami GET/POST do API.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /ApiDemo/GetPost
        /// Wysyła żądanie GET do /posts/1, loguje odpowiedź i przekazuje ją do widoku.
        /// </summary>
        /// <returns>Widok ApiResult z JSON-em odpowiedzi lub komunikatem błędu.</returns>
        public async Task<IActionResult> GetPost()
        {
            // Zmienna do wyświetlania w widoku wykonanej metody (GET)
            ViewBag.Method = "GET";
            try
            {
                // Wykonanie zapytanie GET
                var response = await _httpClient.GetAsync("posts/1");
                response.EnsureSuccessStatusCode();

                // Odczytanie treści odpowiedzi jako string
                string json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("=== API GET RESPONSE ===");
                Console.WriteLine(json);
                Console.WriteLine("========================");

                // Przekazanie odpowiedź do widoku
                ViewBag.Response = json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API GET error: {ex.Message}");
                ViewBag.Response = $"GET error: {ex.Message}";
            }

            return View("ApiResult");
        }


        /// <summary>
        /// GET: /ApiDemo/CreatePost
        /// Wyświetla formularz do wprowadzenia danych, które chcemy wysłać metodą POST.
        /// </summary>
        public IActionResult CreatePost()
        {
            return View();
        }

        /// <summary>
        /// POST: /ApiDemo/CreatePost
        /// Odbiera dane z formularza, serializuje je do JSON, wysyła metodą POST i wyświetla wynik.
        /// </summary>
        /// <param name="postData">Dane przesyłane do API.</param>
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostData postData)
        {
            if (ModelState.IsValid)
            {
                // Zmienna do wyświetlania w widoku wykonanej metody (POST)
                ViewBag.Method = "POST";

                // Serializacja modelu do JSON
                string json;
                try
                {
                    json = JsonSerializer.Serialize(postData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd serializacji JSON: {ex.Message}");
                    ViewBag.Response = $"Błąd serializacji danych: {ex.Message}";
                    return View("ApiResult");
                }

                try
                {
                    // Przygotowanie treści żądania i nagłówków
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Wysyłanie POST
                    var response = await _httpClient.PostAsync("posts", content);
                    response.EnsureSuccessStatusCode();

                    // Odczytanie odpowiedzi jako string
                    string result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("=== API POST RESPONSE ===");
                    Console.WriteLine(result);
                    Console.WriteLine("=========================");

                    ViewBag.Response = result;
                    return View("ApiResult");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"API POST error: {ex.Message}");
                    ViewBag.Response = $"POST error: {ex.Message}";
                    return View("ApiResult");
                }
            }
            // Jeśli walidacja formularza nie przeszła, zwraca formularz z błędami
            return View(postData);
        }
    }
}
