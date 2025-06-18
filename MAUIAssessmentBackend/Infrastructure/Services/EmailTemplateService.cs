using App.Core.Interfaces.IServices;

namespace Infrastructure.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<string> GenerateRegistrationEmail(string firstName, string restaurantName, string email, string password)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='font-family:Segoe UI, sans-serif; background-color:#f4f4f4; margin:0; padding:0;'>
  <div style='max-width:600px; margin:30px auto; background:#ffffff; padding:30px; border-radius:10px; box-shadow:0 2px 8px rgba(0,0,0,0.1);'>
    <div style='background-color:#ff5722; padding:15px; border-radius:10px 10px 0 0; color:white; text-align:center; font-size:24px; font-weight:bold;'>
      Welcome to {restaurantName}!
    </div>
    <div style='margin-top:20px; font-size:16px; color:#333;'>
      <p style='line-height:1.6;'>Hi {firstName},</p>
      <p style='line-height:1.6;'>We're thrilled to have you as a part of our restaurant platform.</p>
      <p style='line-height:1.6;'>Here are your login credentials:</p>
      <ul style='line-height:1.6;'>
        <li><strong>Email:</strong> {email}</li>
        <li><strong>Password:</strong> {password}</li>
      </ul>
      <p style='line-height:1.6;'>You can log in using the button below:</p>
      <a href='https://yourappdomain.com/login' style='display:inline-block; margin-top:20px; padding:12px 24px; background-color:#ff5722; color:#fff; text-decoration:none; border-radius:6px; font-weight:bold;'>Login to Your Account</a>
      <p style='line-height:1.6;'>If you didn’t request this account, you can safely ignore this email.</p>
    </div>
    <div style='margin-top:30px; font-size:13px; color:#888; text-align:center;'>
      &copy; {DateTime.Now.Year} {restaurantName}. All rights reserved.
    </div>
  </div>
</body>
</html>";
        }


    }
}
