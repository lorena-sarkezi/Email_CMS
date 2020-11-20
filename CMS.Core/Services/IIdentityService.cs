using CMS.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
    public interface IIdentityService
    {
        Task<string> LoginUser(LoginModel model);
        Task<RegistrationStatusResponse> RegisterUser(RegisterModel model);
        bool ValidateToken(string token);
    }
}
