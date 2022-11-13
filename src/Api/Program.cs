using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("ConnctionString");
services.AddDbContext<Persistence.DatabaseContext>(opt =>
{
	opt.UseSqlServer(connection);
	opt.EnableSensitiveDataLogging();
});

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
