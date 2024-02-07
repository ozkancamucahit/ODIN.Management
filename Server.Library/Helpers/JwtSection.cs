using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Library.Helpers
{
    public sealed class JwtSection
    {
        public string Key { get; set; } = String.Empty;
        
        public string Issuer { get; set; } = String.Empty;

        public string Audience { get; set; } = String.Empty;

    }
}
