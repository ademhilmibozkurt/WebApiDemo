using ConsumeApi.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumeApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            // create client for request
            var client = _httpClientFactory.CreateClient();
            // httpget request
            var responseMessage = await client.GetAsync("http://localhost:5156/api/products");

            // is response ok
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                // json data object binding - newtonsoft
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductResponseModel>>(jsonData);

                return View(result);
            }
            
            else { return View(null); }
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductResponseModel model) 
        {
            var client = _httpClientFactory.CreateClient();

            // The quickest method of converting between JSON text and a .NET object is using the JsonSerializer.
            // The JsonSerializer converts .NET objects into their JSON equivalent and back again by mapping the .NET object property
            // names to the JSON property names and copies the values for you.
            var jsonData = JsonConvert.SerializeObject(model);

            // A base class representing an HTTP entity body and content headers.
            // Provides HTTP content based on a string.
            StringContent content = new(jsonData, Encoding.UTF8, "application/json");
            // httppost request
            var responseMessage = await client.PostAsync("http://localhost:5156/api/products", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["errorMessage"] = $"Bir hata ile karşılaşıldı! Kod: {(int)responseMessage.StatusCode}";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient();
            // httpget request
            var responseMessage = await client.GetAsync($"http://localhost:5156/api/products/{id}");
            
            if (responseMessage.IsSuccessStatusCode) 
            {
                // take http content
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                // convert json to productresponsemodel object
                var data = JsonConvert.DeserializeObject<ProductResponseModel>(jsonData);
                return View(data);
            }
            else
            {
                TempData["errorMessage"] = $"Bir hata ile karşılaşıldı! Kod: {(int)responseMessage.StatusCode}";
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductResponseModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            // httpput request
            var responseMessage = await client.PutAsync("http://localhost:5156/api/products", content);
        
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["errorMessage"] = $"Bir hata ile karşılaşıldı! Kod: {(int)responseMessage.StatusCode}";
                return View(model);
            }
        }

        public async Task<IActionResult> Remove(int id)
        {
            var client = _httpClientFactory.CreateClient();
            // httpdelete request
            await client.DeleteAsync($"http://localhost:5156/api/products/{id}");

            return RedirectToAction("Index");
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var client = _httpClientFactory.CreateClient();
            // MemoryStream encapsulates data stored as an unsigned byte array. The encapsulated data is directly accessible in memory.
            // Memory streams can reduce the need for temporary buffers and files in an application.
            // The current position of a stream is the position at which the next read or write operation takes place.
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var bytes = stream.ToArray();
            ByteArrayContent content = new(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            MultipartFormDataContent formData = new();
            formData.Add(content, "formFile", file.FileName);

            // httppost request
            await client.PostAsync("http://localhost:5156/api/products/upload", formData);
            return RedirectToAction("Index");
        }
    }
}
