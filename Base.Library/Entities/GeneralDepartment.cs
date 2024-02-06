
namespace Base.Library.Entities
{
    public sealed class GeneralDepartment : BaseEntity, IRelationship
    {
        public IEnumerable<Employee> Employees { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
