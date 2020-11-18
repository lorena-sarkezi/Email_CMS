using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public class LoginModel
    {
        public string EmailOrUsername { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
