using DailyNewsServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Core.Interfaces
{
    public interface IUserService
    {
        User GetUserByEmailAddress(string emailAddress);
        bool Verify(string password, string passwordHash);
    }
}
