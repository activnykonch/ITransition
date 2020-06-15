using System;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string Login { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsBlocked { get; set; }
    }
}
