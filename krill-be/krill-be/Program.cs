using krill_be.Controllers;
using krill_be.Models;
using krill_be.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KrillDatabaseSettings>(
	builder.Configuration.GetSection("KrillDatabase")
	);

builder.Services.Configure<LoginSettings>(
	builder.Configuration.AddJsonFile("config.json").Build().GetSection("loginSettings")
	);

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<UserService>();
builder.Services.AddScoped<LoginSettings>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		// TODO -> deve poi prendere questo valore dalla configurazione presente in config.js
		options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
		options.SlidingExpiration = true;
		options.Cookie.Name = "krill-cookie";
		options.AccessDeniedPath = "/Forbidden/";
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var cookiePolicyOptions = new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Lax
};

app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllers();

app.Run();
