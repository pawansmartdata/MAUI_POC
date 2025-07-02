using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MAUIAssessmentFrontend.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apikey = "AIzaSyC5vf2KQ8pkSQlBov4PaB2ekAG_H7rt5ms";

        public GeminiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetGeminiResponseAsync(string userMessage)
        {
            var endpoint = "https://generativelanguage.googleapis.com/v1/models/gemini-1.5-pro:generateContent?key=AIzaSyC5vf2KQ8pkSQlBov4PaB2ekAG_H7rt5ms";

            string systemPrompt = @"
                                    You are a helpful, professional AI assistant integrated in a mobile app.
                                    Provide clear, user-friendly answers in plain text only.
                                    Do not use Markdown formatting (e.g., no **bold**, no bullet points, no code blocks).
                                    Avoid using *, #, or other special symbols for styling.
                                    Keep responses clean and natural — suitable for mobile display in a simple text box.
                                    ";
            string finalprompt = systemPrompt + "\n\nUser:" + userMessage;
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = finalprompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"❌ Error: {response.StatusCode}\nDetails: {responseBody}";
            }

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var text = doc.RootElement
                              .GetProperty("candidates")[0]
                              .GetProperty("content")
                              .GetProperty("parts")[0]
                              .GetProperty("text")
                              .GetString();

                return text ?? "No response text found.";
            }
            catch (Exception ex)
            {
                return $"❌ Failed to parse response: {ex.Message}\nRaw response: {responseBody}";
            }
        }
    }
}
