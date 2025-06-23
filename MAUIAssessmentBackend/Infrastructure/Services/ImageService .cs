using App.Core.Interfaces.IServices;
using Domain.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvService _envService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IHostingEnvService envService, IHttpContextAccessor httpContextAccessor)
        {
            _envService = envService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> Upload(IFormFile file,string webRootPath)
        {
            try
            {
                if (file == null)
                {
                    return new ResponseDto { Status = 400, Message = "No file provided", Data = "" };
                }

                var validExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
                string extension = Path.GetExtension(file.FileName).ToLower();

                if (!validExtensions.Contains(extension))
                {
                    return new ResponseDto { Status = 400, Message = "Extension not valid", Data = "" };
                }

                if (file.Length > 10 * 1024 * 1024)
                {
                    return new ResponseDto { Status = 400, Message = "Maximum size is 5MB", Data = "" };
                }

                string fileName = Guid.NewGuid().ToString() + extension;
                string uploadPath = Path.Combine(_envService.GetWebRootPath(), "Images");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string filePath = Path.Combine(uploadPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                var request = _httpContextAccessor.HttpContext.Request;
                string fileUrl = $"{request.Scheme}://{request.Host}/Images/{fileName}";

                return new ResponseDto
                {
                    Status = 200,
                    Message = "File Stored Successfully",
                    Data = fileUrl
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Status = 500, Message = $"Server error: {ex.Message}", Data = "" };
            }
        }
    }
}
