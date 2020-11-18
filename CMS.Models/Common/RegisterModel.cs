using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordBase64 { get; set; }
    }
}
