﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.IServices
{
    public interface IJwtService
    {
        Task<string> Authenticate(int userId, string userEmail, string FirstName, string LastName);
    }
}
