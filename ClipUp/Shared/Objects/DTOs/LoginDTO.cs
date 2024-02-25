using System.ComponentModel.DataAnnotations;

namespace ClipUp.Shared.Objects.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Некорректное значение почты")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Укажите пароль")]
        public required string Password { get; set; }
    }
}
