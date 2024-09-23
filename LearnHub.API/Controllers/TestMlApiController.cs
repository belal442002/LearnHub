using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMlApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public TestMlApiController()
        {
            _httpClient = new HttpClient();
        }
        [HttpPost]
        public async Task<IActionResult> TestAPI([FromQuery] string studentAnswer, [FromQuery] string correctAnswer)
        {
            var requestBody = new
            {
                student_answer = studentAnswer,
                professor_answer = correctAnswer
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://splendid-equal-ibex.ngrok-free.app/calculate_similarity", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                if (responseObject.TryGetValue("similarity_score", out JToken similarityToken))
                {
                    var similarityScore = similarityToken.Value<double>();
                    return Ok(similarityScore);
                }
                else
                {
                    return BadRequest("no");
                }
            }
            else
            {
                return BadRequest("no");
            }
        }
    }
}
