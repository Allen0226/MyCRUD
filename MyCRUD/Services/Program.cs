using Microsoft.EntityFrameworkCore;
using Services.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var DataBasestring = builder.Configuration.GetConnectionString("MyDataBase");
builder.Services.AddDbContext<NorthWindContext>(opt =>
{
    opt.UseSqlServer(DataBasestring);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddCors(
    (options) =>
    {
        options.AddPolicy("MyCRUD",
            (builder) =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
    }
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("MyCRUD");
app.MapControllers();

app.Run();
