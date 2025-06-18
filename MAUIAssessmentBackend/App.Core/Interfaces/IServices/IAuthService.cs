using App.Core.Dtos;
using App.Core.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task RegisterAsync(User request,  IFormFile? imageFile,string webRootPath);
       
        Task<Object> LoginAsync(LoginUserDto request);
    }
}
