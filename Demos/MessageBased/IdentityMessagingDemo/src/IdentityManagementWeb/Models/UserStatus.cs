using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagementWeb.Models
{
    public class UserStatus
    {
        public string UserName { get; set; }
        public bool HasUserName => !string.IsNullOrWhiteSpace(UserName);
    }
}
