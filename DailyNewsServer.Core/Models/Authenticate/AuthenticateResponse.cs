using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DailyNewsServer.Core.Models.Authenticate
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.Surname;
            Username = user.EmailAddress;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
