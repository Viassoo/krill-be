using krill_be.Models;
using krill_be.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var applicationConfig = new KrillApplicationSettings();
builder.Configuration.GetSection("applicationSettings").Bind(applicationConfig);

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "OriginsAllowedFromJson", builder =>
	{
		builder.AllowAnyHeader()
		.AllowAnyMethod()
		.WithOrigins(applicationConfig.allowedCrossOrigins.Split(","));
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KrillDatabaseSettings>(
	builder.Configuration.GetSection("KrillDatabase")
	);

builder.Services.Configure<LoginSettings>(
	builder.Configuration.GetSection("loginSettings")
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
		options.Events.OnRedirectToLogin = c =>
		{
			c.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return Task.FromResult<object>(null);
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Authentication", policy =>
	{
		policy.RequireAuthenticatedUser();
	});
});


var app = builder.Build();

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
app.UseCors("OriginsAllowedFromJson");
app.Run();
