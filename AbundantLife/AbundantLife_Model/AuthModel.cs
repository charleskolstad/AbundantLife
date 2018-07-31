using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Model
{
    public class AuthModel
    {
        [Required]
        public string UserName { get; set; }
    }

    public class LoginModel : AuthModel
    {
        [Required]
        public string Password { get; set; }
    }

    public class RecoverModel : AuthModel
    {
        [Required]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")]
        public string Email { get; set; }
    }

    public class CompleteRec : RecoverModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateAccount : AuthModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string RepeatPassword { get; set; }
    }
}
