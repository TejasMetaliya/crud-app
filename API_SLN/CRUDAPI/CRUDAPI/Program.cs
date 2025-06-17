using CRUDAPI.Data;
using CRUDAPI.Middleware;
using CRUDAPI.Repositories;
using CRUDAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using System.Text;

var logger = LogManager.GetCurrentClassLogger();
try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.
	builder.Services.AddScoped<OracleDbContext>();
	builder.Services.AddScoped<UserRepository>();
	builder.Services.AddScoped<ProductRepository>();
	// Register AuthService
	builder.Services.AddScoped<AuthService>();

	// Add CORS policy
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("AllowAngularApp", builder =>
		{
			builder
				.WithOrigins("http://localhost:4200")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
	});

	builder.Services.AddControllers();
	builder.Services.AddSwaggerGen(c =>
	{
		c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

		// Set the comments path for the Swagger JSON and UI
		var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

		// Only include if file exists (avoids error in production)
		if (File.Exists(xmlPath))
		{
			c.IncludeXmlComments(xmlPath);
		}
	});
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
	});

	builder.Services.AddAuthorization();

	try
	{
		using var connection = new OracleConnection(builder.Configuration.GetConnectionString("OracleConnection"));
		await connection.OpenAsync();
		logger.Info("Successfully connected to Oracle database");
	}
	catch (Exception e)
	{
		logger.Error(e, "Failed to connect to Oracle database");
		throw; // This will show the actual error in development
	}

	var app = builder.Build();

	// Use CORS
	app.UseCors("AllowAngularApp");

	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage(); // This shows detailed errors
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Oracle API v1"));
	}

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();
	app.UseRouting();
	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();
	app.UseMiddleware<ExceptionHandlingMiddleware>();

	logger.Info("CRUDAPI Application started.");
	app.Run();
}
catch (Exception e)
{
	logger.Error(e, "CRUDAPI Application failed to start.");
	throw;
}
finally
{
	NLog.LogManager.Shutdown();
}