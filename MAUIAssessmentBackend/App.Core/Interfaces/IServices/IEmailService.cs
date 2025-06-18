using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toE, string name, string subject, string message);
        
    }
}
