﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public enum RegistrationStatusResponse
    {
        USERNAME_EXISTS = 0,
        EMAIL_EXISTS = 1,
        EMAIL_USERNAME_EXIST = 2,
        SUCCESS = 3
    }
}
