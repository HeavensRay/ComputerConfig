using api.Data;
using api.Dto.Config;
using api.Entities;
using api.Interfaces;
using api.Repository;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        } 
    });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    // so it doesnt break me in half when i misspell pasword
    options.Lockout.AllowedForNewUsers = false; 
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBDefault"));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{   // prevent loops!
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
//interface and repo



builder.Services.AddScoped<IConfigRepo, ConfigRepo>(); 

builder.Services.AddScoped<ISSDRepo, SSDRepo>(); 
builder.Services.AddScoped<IGPURepo, GPURepo>(); 
builder.Services.AddScoped<ISSDRepo, SSDRepo>(); 
builder.Services.AddScoped<ICPURepo, CPURepo>(); 
builder.Services.AddScoped<IMoboRepo, MoboRepo>(); 
builder.Services.AddScoped<IPcuRepo, PcuRepo>(); 
builder.Services.AddScoped<IRamRepo, RamRepo>(); 
builder.Services.AddScoped<IGenConfig, GenConfig>(); 
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IComment, CommentRepo>();

// cors for react
builder.Services.AddCors(options =>
{
    Console.WriteLine("CORS middleware loaded");

    options.AddPolicy("Vite", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000") // where react is
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Vite");

app.UseHttpsRedirection(); //nah we going unsafe w this one 

app.MapControllers();

app.Run();

