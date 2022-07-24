using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using N8T.Infrastructure;
using N8T.Infrastructure.Bus;
using N8T.Infrastructure.EfCore;
using N8T.Infrastructure.Middlewares;
using N8T.Infrastructure.Swagger;
using N8T.Infrastructure.Validator;
using OrderingService.Api;
using OrderingService.AppCore;
using OrderingService.AppCore.Services;
using OrderingService.Infrastructure.Data;

const string CorsName = "api";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddCors(options =>
{
    options.AddPolicy(CorsName, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

services.AddHttpContextAccessor();
services.AddCustomMediatR(new[] { typeof(AppCoreAnchor) });
services.AddCustomValidators(new[] { typeof(AppCoreAnchor) });
services.AddControllers().AddMessageBroker(builder.Configuration);
services.AddSwagger(typeof(ApiAnchor));
services.AddPostgresDbContext<MainDbContext>(builder.Configuration.GetConnectionString("postgres"));
services.AddScoped<IOrderRepository, Repository>();

services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidTypes = new[] { "at+jwt" },
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

services.AddHttpClient<UserInfoService>(
    client =>
    {
        var userApiUrl = builder.Configuration["Auth:Authority"];
        client.BaseAddress = new Uri(userApiUrl);
    });

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
app.UseGlobalExceptionHandler();
app.UseSwagger(provider);
app.UseCors(CorsName);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // endpoints.MapControllers().RequireAuthorization("ApiCaller");
    endpoints.MapControllers();
});

app.MigrateDatabase();
app.Run();
