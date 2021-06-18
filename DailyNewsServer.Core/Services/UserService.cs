using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Models;
using DailyNewsServer.Core.Models.Authenticate;
using DailyNewsServer.Core.Models.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DailyNewsServer.Core.Services
{    

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<RefreshToken> _refreshTokenRepository;

        public UserService(IOptions<AppSettings> appSettings, IGenericRepository<User> userRepository, IGenericRepository<RefreshToken> refreshTokenRepository)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
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

        private string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _userRepository.FindByInclude(u => u.EmailAddress == model.Username, u => u.Role).FirstOrDefault();

            // return null if user not found
            if (user == null) return null;

            //return null if password does not match
            if (!Verify(model.Password, user.PasswordHash)) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress, user.UserId);

            // save refresh token
            _refreshTokenRepository.Insert(refreshToken);

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _userRepository.FindBy(u => u.RefreshTokens.Any(t => t.Token == token)).FirstOrDefault();

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = _refreshTokenRepository.FindBy(r => r.Token == token).FirstOrDefault();

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress, user.UserId);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            _refreshTokenRepository.Insert(refreshToken);

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            //var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            var user = _userRepository.FindBy(u => u.RefreshTokens.Any(t => t.Token == token));
            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = _refreshTokenRepository.FindBy(r => r.Token == token).FirstOrDefault();

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _refreshTokenRepository.Update(refreshToken);

            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.All();
        }

        public User GetById(int id)
        {
            return _userRepository.FindByKey(id);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.Description)
            }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress, int userId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    UserId = userId,
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
