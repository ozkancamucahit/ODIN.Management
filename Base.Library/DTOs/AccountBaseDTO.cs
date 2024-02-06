using System.ComponentModel.DataAnnotations;

namespace Base.Library.DTOs
{
    public abstract class AccountBaseDTO
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
