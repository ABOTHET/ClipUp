using ClipUp.Database;
using ClipUp.Shared.Tools.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ClipUp.Shared.Objects.DTOs
{
    public class CreateChannelDTO
    {
        [Required(ErrorMessage = "Укажите имя канала")]
        [UniqueValueValidation(nameof(ChannelName), nameof(ApplicationContext.Channels), "Данное имя для канала уже занято")]
        public required string ChannelName { get; set; }
        public string? Description { get; set; }
    }
}
