using ImageManipulation.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ImageManipulation.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Basic Swagger doc setup
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add the Bearer token definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Make sure swagger UI requires a token to access secure endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
           }
         },
         new string[] {}
       }
    });
});

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
//);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
);

builder.Services.AddTransient<ApplicationDbContext>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); ;
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Basic settings - adjust to your needs
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Read from configuration (appsettings.json or environment vars)
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "InsecureKey123"))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});

app.UseCors();

app.MapControllers();

app.Run();
