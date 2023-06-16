using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System;
using System.Text;
using Taime.Application.Settings;
using Taime.Application.Utils.Data.Api;
using Taime.Application.Utils.Extensions;
using Taime.Application.Utils.Helpers;
using static Taime.Application.Helpers.EnvLoaderHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load Envs
Load();

// Set Data Services
AddDataBaseRepositories(builder.Services);
AddApiCallRepositories(builder.Services);
if (!builder.Services.ExistServiceType<IHttpContextAccessor>())
    builder.Services.AddHttpContextAccessor();

// Set Controllers
builder.Services.AddControllers();

// Set Settings
var settings = new AppSettings();
builder.Services.AddSingleton(settings);

// Set Authentication
var key = Encoding.ASCII.GetBytes(settings.JWTAuthorizationToken);
builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configureOptions =>
{
    configureOptions.RequireHttpsMetadata = true;
    configureOptions.SaveToken = true;
    configureOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Set Api Versioning
builder.Services.AddApiVersioning(setup =>
{
    setup.ReportApiVersions = true;
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.DefaultApiVersion = new ApiVersion(1, 0);
}).AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();

// Set Documentation
builder.Services.AddSwaggerGen(options =>
{
    OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "oauth2"
    };
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.AddSecurityDefinition("Bearer", openApiSecurityScheme);
});
builder.Services.ConfigureOptions<ConfigureSwaggerExtension>();

// Set Auto Inject Services
builder.Services.AddBaseServices();











var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.ConfigObject.DisplayRequestDuration = true;
    options.DocumentTitle = "Swagger - " + GetSwaggerApplicationName();
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();








void AddDataBaseRepositories(IServiceCollection services)
{
    services.AddMySql(GetValueFromEnv<string>("KEY_MYSQL_CONN_STR"));

    var appSettings = new AppSettings();
    services.AddSingleton(appSettings);
}

void AddApiCallRepositories(IServiceCollection services)
{
    services.AddHttpClient();

    var all = ReflectionHelper.ListClassesInherit(typeof(HTTPApiCallRepository));
    foreach (var item in all)
    {
        services.AddDynamicScope(item);
    }
}



string GetSwaggerApplicationName()
{
    var applicationName = PlatformServices.Default.Application.ApplicationName;

    applicationName = applicationName.Split(".")[0];

    return applicationName;
}