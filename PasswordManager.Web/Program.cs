//TODO: Создать авторизацию
//Шифруем данные хэшем от мастер-пароля и его хэша
//Расшифровка будет на стороне клиента
//Когда пользователь хочет поделиться паролем, бэк принимает его в открытом виде, у себя шифрует AESом. Когда логинится пользователь-получатель, перешифруем пароль
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PasswordManager.Database;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PasswordManager",
        Description = "Документация сервиса в голове у Гайнуллина Тимура"
    });
});

builder.Services.AddStorageDbContext(options => options.UseSqlServer(configuration["App:DbConnectionString"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();