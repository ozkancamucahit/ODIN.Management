using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Library.Entities
{
    public sealed class Employee : BaseEntity
    {
        public string CivilId { get; set; } = String.Empty;
        public string FileNUmber { get; set; } = String.Empty;
        public string FullName { get; set; } = String.Empty;
        public string JobName { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Telephonenumber { get; set; } = String.Empty;
        public string Photo { get; set; } = String.Empty;
        public string Other { get; set; } = String.Empty;


        //department relationship many to one

        public GeneralDepartment? GeneralDepartment { get; set; }
        public int GeneralDepartmentId { get; set; }
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }
        public Branch? Branch { get; set; }
        public int BranchId { get; set; }
        public Town? Town { get; set; }
        public int TownId { get; set;}




    }
}
