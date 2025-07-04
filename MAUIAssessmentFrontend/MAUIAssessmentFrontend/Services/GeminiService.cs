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
                                    You are a helpful and professional AI assistant integrated into a .NET MAUI cross-platform mobile app.
                                    The app includes user authentication, profile management, item CRUD operations, and Google Maps integration.
                                    Users can sign up, log in, update profiles, add or edit items (e.g., products, books, animals), and view item locations on a map.
                                    Each item has a name, description, image, and geolocation (latitude/longitude). Items are stored using a repository pattern (e.g., SQLite).
                                    Assist users with clear, concise guidance suitable for a mobile screen. Avoid Markdown, special formatting (*, #, etc.), or code blocks.
                                    Speak in a natural tone and focus on user-friendly instructions.
                                    When helpful, guide users through steps like input validation, uploading images, using maps, or editing items.
                                    Always keep responses short, readable, and clean for display inside a text box.
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
