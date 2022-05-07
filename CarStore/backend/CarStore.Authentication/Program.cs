using CarStore.Authentication.Externals;
using CarStore.Authentication.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//TODO: change UseSqlite to UseSqlServer
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Block 1: Add ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

builder.Services.Configure<IISOptions>(iis =>
{
    iis.AuthenticationDisplayName = "Windows";
    iis.AutomaticAuthentication = false;
});

// Block 2: Add IdentityServer4 with InMemory Configuration

Config.AdminMvcUrl = builder.Configuration["AuthenConfig:AdminMvcUrl"];
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiResources(Config.GetApis())
    .AddInMemoryClients(Config.GetClients())
    .AddAspNetIdentity<ApplicationUser>()
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["AuthenConfig:AuthenticationUrl"];
            options.RequireHttpsMetadata = true;
            options.Audience = "apis";
            options.SaveToken = true;
        });

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "1034266525664-roj66s0m2h44jd8t9k6tlg4kngvd9609.apps.googleusercontent.com";
    options.ClientSecret = "50HdUv41oT0VvH8XJ917NwYG";
});

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.ClientId = "357575163011483";
    options.ClientSecret = "817f9723d8ea7a84cc1412428824e721";
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

builder.Services.AddTransient<IProfileService, ProfileService>();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
