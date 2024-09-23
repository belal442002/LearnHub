using LearnHub.API.Models.Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace LearnHub.API.Helper
{
    public class SimilarityCheckerAPI
    {
        private readonly HttpClient _httpClient;

        public SimilarityCheckerAPI() 
        {
            _httpClient = new HttpClient();
        }
        
        public async Task<bool> CheckSimilarity(string studentAnswer, string correctAnswer)
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
                    if (similarityScore >= 0.7)
                    {
                        return true;
                    }
                }
                else
                {
                    // Invalid response format: 'similarity_score' not found.
                    throw new Exception("invalid response format: 'similarity_score' not found.");
                }
            }
            else
            {
                // response not ok
                throw new Exception("response not ok.");

            }
            return false;
        }
    }
}
