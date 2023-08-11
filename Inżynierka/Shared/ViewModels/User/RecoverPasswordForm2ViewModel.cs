using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.ViewModels.User
{
    public class RecoverPasswordForm2ViewModel
    {
        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Podaj hasło drugi raz.")]
        [CompareProperty(nameof(Password), ErrorMessage = "Podane hasła nie są takie same!")]
        [DataType(DataType.Password)]
        public string SecondPassword { get; set; }
    }
}
