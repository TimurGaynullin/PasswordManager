//TODO: Сделать CRUD операции с паролями, создать модуль шифрования
//Шифруем данные хэшем от мастер-пароля и его хэша
//Расшифровка будет на стороне клиента
//Когда пользователь хочет поделиться паролем, бэк принимает его в открытом виде, у себя шифрует AESом. Когда логинится пользователь-получатель, перешифруем пароль

using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PasswordManager.Database;
using PasswordManager.Web;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/api/Account/login");
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton(new MapperConfiguration(mc =>
{
    
}).CreateMapper());
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PasswordManager",
        Description = "Документация сервиса в голове у Гайнуллина Тимура"
    });
});

//builder.Services.AddStorageDbContext(options => options.UseSqlServer(configuration["App:DbConnectionString"]));

var app = builder.Build();
app.UseAuthentication();
app.UseRouting();
app.UseCors(x=> x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin=>true)
    .AllowCredentials());
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();