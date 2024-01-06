using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
    public class LoginRequest
    {
        public string? UserName { get; set; }

        public string? Passwrod { get; set; }

        public bool RememberMe { get; set; }
    }
}
