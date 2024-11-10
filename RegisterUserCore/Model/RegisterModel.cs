using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegisterUserCore.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="User Name is required")]
        [MaxLength(15)]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string UserName { get; set; }

        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        //unique
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[a-zA-Z]).*$", ErrorMessage = "Maintain Password Standard")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        [Required(ErrorMessage = "Given field is required")]
        [DefaultValue(1)]
        public int IsActive { get; set; } = 1;
    }
}
