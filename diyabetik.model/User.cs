using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace diyabetik.model
{
    public class User
    {   
        public string? Id { get; set; }    

        [Required(ErrorMessage ="İsim alanı boş olamaz")]
        [StringLength(30,MinimumLength =5,ErrorMessage ="En az 5, en fazla 30 karakter girebilirsiniz.")]
        public string adSoyad { get; set; }

        [Required(ErrorMessage ="Mail alanı boş olamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş olamaz")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$", ErrorMessage = "Şifre en az bir harf ve bir rakam içermelidir.")]
        public string Password { get; set; }                
    }
}