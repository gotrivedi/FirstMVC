using Microsoft.IdentityModel.Tokens;
using UserManagment.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "Configuration/permissions.json",
    optional: false,
    reloadOnChange: true
);

var permissions = builder.Configuration
.GetSection("Permissions")
.Get<string[]>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserService>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{

    var secret = builder.Configuration["Jwt:Secret"];
    var key = new SymmetricSecurityKey(Convert.FromBase64String(secret!));

    options.MapInboundClaims = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine(
                $"JWT Authentication Failed: {context.Exception.Message}"
            );

            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            Console.WriteLine("JWT Token Validated");

            foreach (var claim in context.Principal!.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }

            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization(options =>
    {
        foreach (var permission in permissions)
        {
            options.AddPolicy(
                permission,
                policy => policy.RequireClaim("permission", permission));
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
