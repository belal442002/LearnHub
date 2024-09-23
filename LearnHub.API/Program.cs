// for local
//using LearnHub.API.AutoMapper;
//using LearnHub.API.Data;
//using LearnHub.API.Helper;
//using LearnHub.API.Interfaces;
//using LearnHub.API.Models.Domain;
//using LearnHub.API.Repositories;
//using LearnHub.API.UniteOfWork;
//using LearnHub.AutoMapper;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//// Configure WebRootPath explicitly
//// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();
//// new for use tokens in swagger
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Learn_Hub_Api",
//        Version = "v1"

//    });
//    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = JwtBearerDefaults.AuthenticationScheme,
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = JwtBearerDefaults.AuthenticationScheme
//                },
//                //Scheme = "Oauth2",
//                Scheme = "Bearer",
//                Name = JwtBearerDefaults.AuthenticationScheme,
//                In = ParameterLocation.Header
//            },
//            new List<String>()
//        }
//    });
//});
//// new to use notifications
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<FileService>();
//builder.Services.AddScoped<StudentService>();
//builder.Services.AddScoped<MappingProfileManual>();
//builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//builder.Services.AddScoped<ITokenRepository, TokenRepository>();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//// add auto mapper
//builder.Services.AddAutoMapper(typeof(MappingProfile));
//// add db context
//builder.Services.AddDbContext<LearnHubDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("LearnHupConnectionString"));
//});
//// add identity
//builder.Services.AddIdentityCore<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("LearnHub")
//    .AddEntityFrameworkStores<LearnHubDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequiredLength = 6;
//    options.Password.RequiredUniqueChars = 0;
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
//    AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//        };
//        // new to Allow SignalR access tokens to be passed through query string
//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                var accessToken = context.Request.Query["access_token"];
//                var path = context.HttpContext.Request.Path;
//                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
//                {
//                    context.Token = accessToken;
//                }
//                return Task.CompletedTask;
//            }
//        };
//    });

//builder.Services.AddCors(corsOptions =>
//{
//    corsOptions.AddPolicy("MyPolicy", corsPolicyBuilder =>
//    {
//        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

//    });
//});

//builder.Services.AddSignalR();

//builder.Services.AddHttpClient();

//var app = builder.Build();

////Configure the HTTP request pipeline use this when run from local
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// use this when run on cloud
////app.UseSwagger();
////app.UseSwaggerUI(c =>
////{
////    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnHub API V1");
////    c.RoutePrefix = string.Empty;  // Set Swagger UI at the app's root
////});

//app.UseStaticFiles();
////app.UseStaticFiles(new StaticFileOptions
////{
////    FileProvider = new PhysicalFileProvider(@"D:/Sites/site6168/wwwroot/Uploads"),
////    RequestPath = "/wwwroot/Uploads"
////});
//app.UseCors("MyPolicy"); // before routing to allow external consumer use api
//app.UseRouting();
//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();
//// new to use notification
//app.MapHub<NotificationHub>("/notificationHub");
//app.Run();



using LearnHub.API.AutoMapper;
using LearnHub.API.Data;
using LearnHub.API.Helper;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using LearnHub.API.Repositories;
using LearnHub.API.UniteOfWork;
using LearnHub.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Configure WebRootPath explicitly
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// new for use tokens in swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Learn_Hub_Api",
        Version = "v1"

    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                //Scheme = "Oauth2",
                Scheme = "Bearer",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<String>()
        }
    });
});
// new to use notifications
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<MappingProfileManual>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// add auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// add db context
builder.Services.AddDbContext<LearnHubDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LearnHupConnectionString"));
});
// add identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("LearnHub")
    .AddEntityFrameworkStores<LearnHubDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        // new to Allow SignalR access tokens to be passed through query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("MyPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });
});

builder.Services.AddSignalR();

builder.Services.AddHttpClient();

var app = builder.Build();

//Configure the HTTP request pipeline use this when run from local
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// use this when run on cloud
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnHub API V1");
    c.RoutePrefix = string.Empty;  // Set Swagger UI at the app's root
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"D:/Sites/site6168/wwwroot/Uploads"),
    RequestPath = "/wwwroot/Uploads"
});
app.UseCors("MyPolicy"); // before routing to allow external consumer use api
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// new to use notification
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
