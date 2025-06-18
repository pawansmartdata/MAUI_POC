using App.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Hosting;

namespace MAUIAssessmentBackend.External
{
    public class HostingEnvService : IHostingEnvService
    {
        private readonly IWebHostEnvironment _env;

        public HostingEnvService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetWebRootPath()
        {
            return _env.WebRootPath ?? "wwwroot"; // fallback if null
        }
    }
}
