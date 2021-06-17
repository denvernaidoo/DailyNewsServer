using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Core.Models.Login
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
