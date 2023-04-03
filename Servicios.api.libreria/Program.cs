using Servicios.api.libreria.Core;
using Servicios.api.libreria.Core.ContextMongoDb;
using Servicios.api.libreria.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//mongodb configuration
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoDb")
);

// Agregando singleton con mongoo para que la configuracion sea 1
builder.Services.AddSingleton<MongoSettings>();

// Inyectando author context modo tracient.. osea periodos cortos de isntancias.
builder.Services.AddTransient<IAuthorContext, AuthorContext>();

// inyectando servicio o repositorio
builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();
// Inyectando servicio generico - trabaja cada vez que un clente se haga un request.
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));


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
