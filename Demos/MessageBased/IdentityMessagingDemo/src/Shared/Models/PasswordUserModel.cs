using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class PasswordUserModel:UserNameModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
