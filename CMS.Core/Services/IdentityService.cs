using CMS.Data;
using CMS.Data.Common;
using CMS.Data.Database;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
    public class IdentityService :IIdentityService
    {
        private readonly CmsDbContext cmsDbContext;
        private readonly IConfiguration configuration;

        public IdentityService(CmsDbContext cmsDbContext, IConfiguration configuration)
        {
            this.cmsDbContext = cmsDbContext;
            this.configuration = configuration;
        }

        public async Task<string> LoginUser(LoginModel model)
        {

            string password = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(model.Password));

            User user = cmsDbContext.Users.FirstOrDefault(x => x.Username == model.EmailOrUsername || x.Email == model.EmailOrUsername);

            if (user == null) return null;
            
            byte[] saltedPasswordByteArr = new Rfc2898DeriveBytes(Convert.FromBase64String(model.Password), Encoding.UTF8.GetBytes(user.PasswordSalt), 1000).GetBytes(128);

            if (Convert.ToBase64String(saltedPasswordByteArr) != user.PasswordHash)
                return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(configuration["ClientSecret"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, $"{user.FirstName} {user.LastName}"),
                //new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RegistrationStatusResponse> RegisterUser(RegisterModel model)
        {
            int emailCount = await cmsDbContext.Users.Where(x => x.Email == model.Email).CountAsync();
            if (emailCount > 0) return RegistrationStatusResponse.EMAIL_EXISTS;

            int usernameCount = await cmsDbContext.Users.Where(x => x.Username == model.Username).CountAsync();
            if (usernameCount > 0) return RegistrationStatusResponse.USERNAME_EXISTS;



            string submittedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(model.PasswordBase64));
            string passwordSalt = string.Empty;
            string passwordHash = string.Empty;

            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                passwordSalt = new Guid(bytes).ToString();
            }

            byte[] passwordBytes = new Rfc2898DeriveBytes(submittedPassword, Encoding.UTF8.GetBytes(passwordSalt), 1000).GetBytes(128);
            passwordHash = Convert.ToBase64String(passwordBytes);

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            cmsDbContext.Users.Add(user);
            await cmsDbContext.SaveChangesAsync();

            return RegistrationStatusResponse.SUCCESS;

        }
    }
}
