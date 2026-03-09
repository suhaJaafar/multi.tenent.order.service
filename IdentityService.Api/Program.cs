
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
// Register DbContext
builder.Services.AddDbContext< IdentityService.Domain.DBContexts.OSContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// Register Application layer services (MediatR, etc.)
builder.Services.AddApplication();

// Register Infrastructure layer services (Repositories, UnitOfWork, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Register HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped< IdentityService.Infrastructure.Interfaces.IUserService,IdentityService.Application.UserServices.UserService>();
builder.Services.AddScoped<IdentityService.Infrastructure.Interfaces.IProductService, IdentityService.Application.ProductServices.ProductService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Installment Management API",
        Version = "v1",
        Description = "API for managing installments"
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

    // Maintain controller/tag ordering via custom document filter
    options.DocumentFilter<SwaggerTagOrderDocumentFilter>();
    
    // Hide ActiveUserData parameter from Swagger UI (it's populated from JWT automatically)
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
