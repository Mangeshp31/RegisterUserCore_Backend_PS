using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RegisterUserCore.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Password is required")]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int IsActive { get; set; } = 1;
    }
}
