using System.Net.Http.Json;
using System.Net.NetworkInformation;
using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;


namespace MAUIAssessmentFrontend.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {

            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            }
            var token = new TokenDataDto
            {
                Status = (int)(response.StatusCode)

            };
            return new AuthResponseDto
            {
                Token = token  
            };
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {

            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(registerDto.FirstName), "FirstName");
            content.Add(new StringContent(registerDto.LastName), "LastName");
            content.Add(new StringContent(registerDto.Email), "Email");
            content.Add(new StringContent(registerDto.Password), "Password");
            //content.Add(new StringContent(registerDto.PhoneNumber, 0));
            content.Add(new StringContent(registerDto.PhoneNumber), "PhoneNumber");

            if (registerDto.ProfilePictureStream != null && registerDto.ProfilePictureFileName != null)
            {
                var imageContent = new StreamContent(registerDto.ProfilePictureStream);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // adjust if needed
                content.Add(imageContent, "ProfilePicture", registerDto.ProfilePictureFileName);
            }

            var response = await _httpClient.PostAsync("api/Auth/signup", content);

            //var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerDto);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
