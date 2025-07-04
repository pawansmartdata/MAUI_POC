using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
        public async Task<UpdateUserResponseDto> UpdateProfileAsync(int userId, string firstName, string lastName, string email, string phoneNumber, string ProfilePicture)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                form.Add(new StringContent(firstName), "FirstName");
                form.Add(new StringContent(lastName), "LastName");
                form.Add(new StringContent(email), "Email");
                form.Add(new StringContent(phoneNumber ?? ""), "PhoneNumber");

                if (ProfilePicture != null)
                {
                    var stream = File.OpenRead(ProfilePicture);
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    form.Add(fileContent, "ProfilePicture", Path.GetFileName(ProfilePicture));
                }
                 
                var response = await _httpClient.PutAsync($"api/User/{userId}", form);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<UpdateProfileApiResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    var profile = result?.ProfileData ?? new UpdateUserResponseDto();
                    profile.Status = result?.Status ?? (int)response.StatusCode;
                    return profile;
                }
               
                    else
                    {
                        return new UpdateUserResponseDto
                        {
                            Status = (int)response.StatusCode,
                            Message = "Error updating profile."
                        };
                    }
                
                            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] UpdateProfileAsync error: {ex.Message}");
                return new UpdateUserResponseDto
                {
                    Message = ex.Message
                };
            }
        }

    }
}

