using DailyNewsServer.Core.Models;
using DailyNewsServer.Core.Models.Authenticate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Core.Interfaces
{
    public interface IUserService
    {
        User GetUserByEmailAddress(string emailAddress);
        bool Verify(string password, string passwordHash);
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
        List<RefreshToken> GetAllRefreshTokens(int userId);
    }
}
