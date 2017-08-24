using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TestatServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Account Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
