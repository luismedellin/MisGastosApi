using MisGastosApi.Core.Services;
using dotenv.net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MisGastosApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using MisGastosApi.Middlewares;
using MisGastosApi.Data.Repositories;
using MisGastosApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((configBuilder) =>
{
    //configBuilder.Sources.Clear();
    DotEnv.Load();
    configBuilder.AddEnvironmentVariables();
});

// Add services to the container.

#region Libraries Configuration
builder.Services.AddSingleton<DapperContext>();

#endregion
#region My Services
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IMessageService, MessageService>();
#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            builder.Configuration.GetValue<string>("CLIENT_ORIGIN_URL"))
            .WithHeaders(new string[] {
                HeaderNames.ContentType,
                HeaderNames.Authorization,
            })
            .AllowAnyMethod()
            .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureServices(services =>
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var audience =
                  builder.Configuration.GetValue<string>("AUTH0_AUDIENCE");

            options.Authority =
                  $"https://{builder.Configuration.GetValue<string>("AUTH0_DOMAIN")}/";
            options.Audience = audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("read:admin-messages", policy =>
        {
            policy.Requirements.Add(new RbacRequirement("read:admin-messages"));
        });
    });

    services.AddSingleton<IAuthorizationHandler, RbacHandler>();
});

var app = builder.Build();

var requiredVars =
    new string[] {
          "PORT",
          "CLIENT_ORIGIN_URL",
          "AUTH0_DOMAIN",
          "AUTH0_AUDIENCE",
    };

foreach (var key in requiredVars)
{
    var value = app.Configuration.GetValue<string>(key);

    if (value == "" || value == null)
    {
        throw new Exception($"Config variable missing: {key}.");
    }
}

//app.Urls.Add(
//    $"https://+:{app.Configuration.GetValue<string>("PORT")}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();
app.UseSecureHeaders();
app.MapControllers();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
