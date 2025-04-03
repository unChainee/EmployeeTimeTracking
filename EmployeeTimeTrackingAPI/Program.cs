using EmployeeTimeTrackingAPI;
using EmployeeTimeTrackingAPI.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Dodajemy Basic Authentication
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Basic", options => { });

// Us³ugi autoryzacji
builder.Services.AddAuthorization();

// Us³uga dla po³¹czenia z baz¹ danych
builder.Services.AddSingleton<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja repozytoriów
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

// Inicjalizacja bazy danych
builder.Services.AddSingleton<DatabaseInitializer>();

// Dodanie kontrolerów
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Konfiguracja Swaggera
builder.Services.AddSwaggerGen(c =>
{
    // Dodajemy wsparcie dla Basic Auth w Swaggerze
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic Authentication header using the format 'Basic base64(username:password)'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Middleware autoryzacji
app.UseAuthentication();
app.UseAuthorization();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
databaseInitializer.Initialize();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
