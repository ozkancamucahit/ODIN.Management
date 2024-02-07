using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Library.DTOs
{
    public sealed class RefreshToken
    {
        [Required]
        public string Token { get; set; } = String.Empty;
    }
}
