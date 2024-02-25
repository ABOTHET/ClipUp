namespace ClipUp.Shared.Objects.Interfaces
{
    public interface IPictureService
    {
        public Task<bool> SaveAsWebpAsync(string path);
        public Task<Guid> CreatePictureAsync(Guid id, string path);
    }
}
