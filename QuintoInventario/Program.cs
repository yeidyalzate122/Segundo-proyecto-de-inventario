
using Microsoft.EntityFrameworkCore;
using QuintoInventario.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



//Concexion base de datos
//tiene que ir ariba de var app siempre 
builder.Services.AddDbContext<InventarioContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var proveerdor = builder.Services.BuildServiceProvider();
var configuracion = proveerdor.GetRequiredService<IConfiguration>();

builder.Services.AddCors(opciones =>
{
    var frontendURL = configuracion.GetValue<String>("frontend_url");
    opciones.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
