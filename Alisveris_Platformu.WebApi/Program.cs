using Alisveris_Platformu.Business.DataProtection;
using Alisveris_Platformu.Business.Operations.Order;
using Alisveris_Platformu.Business.Operations.Product;
using Alisveris_Platformu.Business.Operations.Setting;
using Alisveris_Platformu.Business.Operations.User;
using Alisveris_Platformu.Data.Context;
using Alisveris_Platformu.Data.Repositories;
using Alisveris_Platformu.Data.UnitOfWork;
using Alisveris_Platformu.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY__** your JWT Bearer Token on Texbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,

        }

    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>() }
    });

});

builder.Services.AddScoped<IDataProtection, DataProtection>();

var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath,
    "App_Data", "Keys"));   


builder.Services.AddDataProtection()
    .SetApplicationName("Alisveris_Platformu")
    .PersistKeysToFileSystem(keysDirectory);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (builder.Configuration["Jwt:SecretKey"]!))
        };


    });



var connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddDbContext<Alisveris_PlatformuDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<IOrderService, OrderManager>();

builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddScoped<ISettingService, SettingManager>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMaintenanceMode();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
