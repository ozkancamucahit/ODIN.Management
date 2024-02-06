

using System.Text.Json.Serialization;

namespace Base.Library.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        

    }
}
