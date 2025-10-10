using Asp.Versioning.ApiExplorer;
using MyProject.Api.Extensions;
using MyProject.API.Extensions;
using MyProject.API.Middleware;
using MyProject.Application.DependencyInjection;
using MyProject.Contracts.Localization;
using MyProject.Domain.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddControllers(options =>
{
    // Suppress automatic model validation to use custom validation behavior
    options.ModelValidatorProviders.Clear();
}).AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new UlidJsonConverter()); });
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerDependencies();
builder.Services.AddVersioning();
builder.Services.AddLocalizationServices();
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseMiddleware<ExceptionHandler>();
app.UseLocalizationMiddleware();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
// app.MapOpenApi(); 
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            $"API {description.GroupName.ToUpperInvariant()}");
});
// }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();