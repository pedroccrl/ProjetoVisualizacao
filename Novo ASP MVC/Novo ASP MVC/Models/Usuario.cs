using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models
{
    public class Usuario
    {   
        [Required(ErrorMessage = "Usuário é obrigatório")]
        [Display(Name = "Usuário")]
        public string User { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        
        [MinLength(6)]
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; }
    }
}