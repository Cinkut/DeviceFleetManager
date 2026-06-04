var builder = WebApplication.CreateBuilder(args);

// Rejestracja kontrolerów i generatora dokumentacji OpenAPI (Swagger).
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI dostępne w środowisku deweloperskim pod /swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
