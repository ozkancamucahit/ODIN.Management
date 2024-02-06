using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Library.DTOs
{
    public sealed class RegisterDTO: AccountBaseDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; } = String.Empty;


    }
}
