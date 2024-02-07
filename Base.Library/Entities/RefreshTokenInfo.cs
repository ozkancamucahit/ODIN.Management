using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Library.Entities
{
    public sealed class RefreshTokenInfo
    {
        public int Id { get; set; }

        public string Token { get; set; } = String.Empty;

        public int UserId { get; set; }
    }
}
