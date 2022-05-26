using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using N8T.Infrastructure;
using N8T.Infrastructure.Bus;
using N8T.Infrastructure.EfCore;
using N8T.Infrastructure.Swagger;
using N8T.Infrastructure.Validator;
using ProductService.Api;
using ProductService.AppCore;
using ProductService.Infrastructure.Data;
using ApiAnchor = ProductService.Api.V1.Anchor;

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
services.AddCustomMediatR(new[] { typeof(Anchor) });
services.AddCustomValidators(new[] { typeof(Anchor) });
services.AddControllers().AddMessageBroker(builder.Configuration);
services.AddSwagger(typeof(ApiAnchor));
services.AddPostgresDbContext<MainDbContext>(builder.Configuration.GetConnectionString("postgres"));
services.AddScoped<IRepository, Repository>();

services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        // Todo: configuration
        options.Authority = "https://localhost:7280";
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidTypes = new[] { "at+jwt" },
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
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
