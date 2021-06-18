using DailyNewsServer.Core.Models.Authenticate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DailyNewsServer.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
