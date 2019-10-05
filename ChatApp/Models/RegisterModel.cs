using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50,ErrorMessage ="Username must be between {2} and {1} characters long",MinimumLength =6)]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage ="Password must be between {2} and {1} character long", MinimumLength =6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }

    }
}
