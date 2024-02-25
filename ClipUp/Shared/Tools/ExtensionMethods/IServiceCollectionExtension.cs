using ClipUp.Database;
using ClipUp.Services;
using ClipUp.Shared.Objects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Tools.ExtensionMethods
{
    public static class IServiceCollectionExtension
    {
        public static void AddApplicationContext(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            // Старая поддержка DateTime для postgres
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // Получаем конфиг
            string settings = configuration.GetConnectionString("DefaultConnection")!;
            // Подключаем
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(settings));
        }
        public static void AddProfileService(this IServiceCollection services)
        {
            services.AddScoped<IProfileService, ProfileService>();
        }
        public static void AddJwtService(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
        }
        public static void AddPictureService(this IServiceCollection services)
        {
            services.AddScoped<IPictureService, PictureService>();
        }
    }
}
