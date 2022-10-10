using Microsoft.AspNetCore.Identity;

namespace Identity.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; } //lista de la tabla intermedia user roles
    }
}
