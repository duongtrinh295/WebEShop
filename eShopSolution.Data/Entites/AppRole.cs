using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entites
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
