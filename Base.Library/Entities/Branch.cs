using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Library.Entities
{
    public sealed class Branch : BaseEntity, IRelationship
    {
        public IEnumerable<Employee> Employees { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
