//"DbConnectionString": "Server=localhost\\SQLEXPRESS02;Database=PasswordManager;Trusted_Connection=True;",

using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PasswordManager.Database;
using PasswordManager.Domain;
using PasswordManager.Domain.Abstractions;
using PasswordManager.Web;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.
var  myAllowSpecificOrigins = "_myAllowSpecificOrigins";
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:8080/");
        });
});*/

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/api/Account/login");
    });
builder.Services.AddAuthorization();

builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddTransient<ISecretDataService, SecretDataService>();
builder.Services.AddTransient<IDataTypeService, DataTypeService>();
builder.Services.AddTransient<IAesProtector, AesProtector>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IHasher, Hasher>();
builder.Services.AddSingleton(new MapperConfiguration(mc =>
{
    mc.AddProfile(new ControllersMappingProfile());
    mc.AddProfile(new MappingProfile());
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

builder.Services.AddStorageDbContext(options => options.UseSqlServer(configuration["App:DbConnectionString"]));

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