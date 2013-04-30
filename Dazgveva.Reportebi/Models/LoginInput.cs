using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Dazgveva.Reportebi.Models
{
    public class LoginInput
    {
        [Required]
        [Display(Name = "მომხმარებელი:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [Display(Name = "პაროლი:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}