using ClipUp.Database;
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
    }
}
