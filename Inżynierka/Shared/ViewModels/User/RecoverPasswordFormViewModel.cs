using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.ViewModels.User
{
    public class RecoverPasswordFormViewModel
    {
        [Required(ErrorMessage = "Pole \"Email\" jest wymagane!")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Podana wartość nie jest poprawnym adresem email!")]
        public string Email { get; set; }
    }
}
