using MAUIAssessmentFrontend.Models;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);
    }
}
