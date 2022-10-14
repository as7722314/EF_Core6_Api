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
        ValidateIssuer = true, //是否驗證Issuer
        ValidIssuer = configuration["Jwt:Issuer"], //發行人Issuer
        ValidateAudience = true, //是否驗證Audience
        ValidAudience = configuration["Jwt:Audience"], //訂閱人Audience
        ValidateIssuerSigningKey = true, //是否驗證SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
        ValidateLifetime = true, //是否驗證失效時間
        ClockSkew = TimeSpan.FromSeconds(30), //過期時間容錯值，解決伺服器端時間不同步問題（秒）
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

// 註冊服務
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