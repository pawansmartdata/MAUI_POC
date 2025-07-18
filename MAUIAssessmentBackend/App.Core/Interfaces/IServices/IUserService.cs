﻿using App.Core.Dtos;
using App.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.IServices
{
    public interface IUserService
    {
        Task<Object> GetUserByIdAsync(int userId);
        Task<UpdateUserResponseDto> UpdateUserAsync(int userId, UpdateUserDto updateUserDto, string webRootPath);
        Task<bool> DeleteUserAsync(int userId);
    }
}
