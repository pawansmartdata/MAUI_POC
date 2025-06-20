using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProfileResponseDto?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/User/user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Error: {ex.Message}");
            }

            return null;
        }
        public async Task<bool> UpdateProfileAsync(int userId, string firstName, string lastName, string email, string phoneNumber, FileResult imageFile)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                form.Add(new StringContent(firstName), "FirstName");
                form.Add(new StringContent(lastName), "LastName");
                form.Add(new StringContent(email), "Email");
                form.Add(new StringContent(phoneNumber ?? ""), "PhoneNumber");

                if (imageFile != null)
                {
                    var stream = await imageFile.OpenReadAsync();
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    form.Add(fileContent, "ProfileImage", imageFile.FileName);
                }

                var response = await _httpClient.PutAsync($"api/User/{userId}", form);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] UpdateProfileAsync error: {ex.Message}");
                return false;
            }
        }

    }
}
