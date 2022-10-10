using Microsoft.AspNetCore.Identity;

namespace Identity.Domain
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; } //lista de la tabla intermedia user roles
    }
}
