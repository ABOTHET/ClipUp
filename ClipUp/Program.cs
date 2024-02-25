using ClipUp.Shared.Tools.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

// Переменные
ConfigurationManager сonfiguration = builder.Configuration;

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddApplicationContext(сonfiguration);
builder.Services.AddProfileService();
builder.Services.AddJwtService();
builder.Services.AddPictureService();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Собираем
var app = builder.Build();

// Если режим Development, то включаем Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
