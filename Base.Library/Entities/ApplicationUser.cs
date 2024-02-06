using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Library.Entities
{
    public sealed class ApplicationUser: BaseEntity
    {
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

    }
}
