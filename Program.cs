using CoreApiTest.Data;
using CoreApiTest.Exceptions;
using CoreApiTest.Interface;
using CoreApiTest.Middleware;
using CoreApiTest.Models;
using CoreApiTest.Resource.Helpers;
using CoreApiTest.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Configuration;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add(typeof(ExceptionFilter));
    }
).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

//builder.Services.AddDbContext<CoreApiTestContext>(
//        options => options.UseSqlServer(configuration["ConnectionStrings:default"]));
builder.Services.AddDbContext<CoreApiTestContext>();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddHostedService<TimerService>();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();

//JWT
/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //�O�_����Issuer
        ValidIssuer = configuration["Jwt:Issuer"], //�o��HIssuer
        ValidateAudience = true, //�O�_����Audience
        ValidAudience = configuration["Jwt:Audience"], //�q�\�HAudience
        ValidateIssuerSigningKey = true, //�O�_����SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
        ValidateLifetime = true, //�O�_���ҥ��Įɶ�
        ClockSkew = TimeSpan.FromSeconds(30), //�L���ɶ��e���ȡA�ѨM���A���ݮɶ����P�B���D�]��^
        RequireExpirationTime = true,
    };
});
*/

//Google Auth

builder.Services
    .AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
    .AddCookie();
/*
.AddGoogle(
googleOptions =>
    {
        googleOptions.ClientId = configuration["Google:ClientId"];
        googleOptions.ClientSecret = configuration["Google:ClientSecret"];
        googleOptions.CallbackPath = "/auth/success";
    });
*/

/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = configuration["Google:Authority"];
        options.ClientId = configuration["Google:ClientId"];
        options.ClientSecret = configuration["Google:ClientSecret"];
        options.ResponseType = "code";

        options.Scope.Add("openid");
        options.Scope.Add("profile");

        options.SaveTokens = true;
    });
*/

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*");
                          policy.WithHeaders("*");
                          policy.WithMethods("*");
                      });
});

//Hangfire Service
/*
string dbConnectionString = configuration["ConnectionStrings:default"];
builder.Services.AddHangfire(configuration => configuration
.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(dbConnectionString, new SqlServerStorageOptions
{
    CommandBatchMaxTimeout = TimeSpan.FromSeconds(5),
    SlidingInvisibilityTimeout = TimeSpan.FromSeconds(5),
    QueuePollInterval = TimeSpan.Zero,
    UseRecommendedIsolationLevel = true,
    DisableGlobalLocks = true
}));
*/
//builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration["ConnectionStrings:default"]));
//builder.Services.AddHangfireServer();

// ���U�A��
//builder.Services.AddSingleton<JwtHelpers>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ToOrderApiResource>();
builder.Services.AddTransient<ToUserApiResource>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseTestMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();