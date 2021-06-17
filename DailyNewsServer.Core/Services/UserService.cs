using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Models;
using DailyNewsServer.Core.Models.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailyNewsServer.Core.Services
{    

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private IGenericRepository<User> _userRepository;
        public UserService(IOptions<AppSettings> appSettings, IGenericRepository<User> userRepository)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        public User GetUserByEmailAddress(string emailAddress)
        {
            var user = _userRepository.FindByInclude(u => u.EmailAddress == emailAddress, u => u.Role).FirstOrDefault();
            return user;
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
