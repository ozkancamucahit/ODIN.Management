using System.Text.Json.Serialization;

namespace Base.Library.Entities
{
    public interface IRelationship
    {
        [JsonIgnore]
        public IEnumerable<Employee> Employees { get; set; }
    }
}
