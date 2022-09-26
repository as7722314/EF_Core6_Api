using CoreApiTest.Data;
using CoreApiTest.Interface;
using CoreApiTest.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CoreApiTest.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using CoreApiTest.Resource.Helpers;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);

//builder.Services.AddDbContext<CoreApiTestContext>(
//        options => options.UseSqlServer(configuration["ConnectionStrings:default"]));
builder.Services.AddDbContext<CoreApiTestContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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

// 註冊服務
builder.Services.AddSingleton<JwtHelpers>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ToOrderApiResource>();
builder.Services.AddTransient<ToUserApiResource>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
