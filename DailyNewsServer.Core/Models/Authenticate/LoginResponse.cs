using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Core.Models.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
    }
}
