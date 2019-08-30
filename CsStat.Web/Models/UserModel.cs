using System.ComponentModel.DataAnnotations;

namespace CsStat.Web.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Required field")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Password { get; set; }
    }
}