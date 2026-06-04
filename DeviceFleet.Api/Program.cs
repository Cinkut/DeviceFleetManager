using DeviceFleet.Application;
using DeviceFleet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja warstw aplikacji.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Kontrolery + dokumentacja OpenAPI (Swagger).
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
