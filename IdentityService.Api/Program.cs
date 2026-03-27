
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IdentityService.Api.Attributes;
using IdentityService.Api.Extensions;
using IdentityService.Api.Swagger;
using IdentityService.Application;
using IdentityService.Domain.Identity.ObjectValues;
using IdentityService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    {
        // Register the ActiveUser model binder
        options.ModelBinderProviders.Insert(0, new ActiveUserModelBinderProvider());
    })
    .ConfigureApiBehaviorOptions(_ =>
    {
        // Enable case-insensitive property name binding
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new EmailJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new PasswordJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new PhoneNumberJsonConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// Register Application layer service
builder.Services.AddApplication();

// Register Infrastructure layer services 
builder.Services.AddInfrastructure(builder.Configuration);

// Register HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Identity Service API",
        Version = "v1",
        Description = "API for managing users and authentication"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Just Enter the JWT token.\n\nExample: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });

    options.DocumentFilter<SwaggerTagOrderDocumentFilter>();
    
    options.OperationFilter<HideActiveUserParameterFilter>();
});

// JWT auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});

var app = builder.Build();
app.EnsureDbIsCreated();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
