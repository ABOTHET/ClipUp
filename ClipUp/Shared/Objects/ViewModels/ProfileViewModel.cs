using ClipUp.Shared.Objects.Models;

namespace ClipUp.Shared.Objects.ViewModels
{
    public class ProfileViewModel
    {
        public Guid? Id { get; set; }
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? AvatarId { get; set; }
        
        public ProfileViewModel(Profile profile)
        {
            Id = profile?.Id;
            Login = profile?.DataProfile?.Login;
            Name = profile?.DataProfile?.Name;
            Surname = profile?.DataProfile?.Surname;
            DateOfBirth = profile?.DataProfile?.DateOfBirth;
            AvatarId = profile?.DataProfile?.Avatar?.Id;
        }
    }
}
