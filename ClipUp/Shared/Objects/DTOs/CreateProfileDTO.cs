using ClipUp.Shared.Tools.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using ClipUp.Database;

namespace ClipUp.Shared.Objects.DTOs
{
    public class CreateProfileDTO
    {
        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Некорректное значение почты")]
        [UniqueValueValidation(nameof(Email), nameof(ApplicationContext.Profiles), "Эта почта уже занята")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(8, ErrorMessage = "Минимальная длина пароля 8 символов")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Укажите логин")]
        [UniqueValueValidation(nameof(Login), nameof(ApplicationContext.DataProfiles), "Этот логин уже занят")]
        public required string Login { get; set; }
        [Required(ErrorMessage = "Укажите имя")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Укажите фамилию")]
        public required string Surname { get; set; }
        [Required(ErrorMessage = "Укажите дату рождения")]
        public required DateTime DateOfBirth { get; set; }
    }
}
