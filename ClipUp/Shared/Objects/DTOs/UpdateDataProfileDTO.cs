using ClipUp.Database;
using ClipUp.Shared.Tools.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ClipUp.Shared.Objects.DTOs
{
    public class UpdateDataProfileDTO
    {
        [UniqueValueValidation(nameof(Login), nameof(ApplicationContext.DataProfiles), "Этот логин уже занят")]
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
