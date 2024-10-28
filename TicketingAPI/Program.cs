using TicketingAPI.Services.Eventos;
using TicketingAPI.Services.Logs;
using TicketingAPI.Services.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar AuthService al contenedor de dependencias
builder.Services.AddScoped<AuthService>();

// Si tambi�n necesitas LogService, agr�guelo aqu�
builder.Services.AddScoped<LogService>();

builder.Services.AddScoped<UserService>(); // O usa AddTransient o AddSingleton seg�n el tiempo de vida necesario.
builder.Services.AddScoped<CategoriasService>();
builder.Services.AddScoped<EventosService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
