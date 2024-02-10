using ClipUp.Shared.Tools.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

// ����������
ConfigurationManager �onfiguration = builder.Configuration;

// ��������� �������
builder.Services.AddControllers();
builder.Services.AddApplicationContext(�onfiguration);

// ��������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������
var app = builder.Build();

// ���� ����� Development, �� �������� Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
