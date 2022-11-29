using EXAMPLE.API.Access.Control.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using EXAMPLE.API.Access.Control;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Adds support for JSONPatch. https://jsonpatch.com/
    options.InputFormatters.Insert(0, JsonFormatter.GetJsonPatchInputFormatter());
}).AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

// Configure swagger.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        // Basic information about the API and who to contact.
        Version = "1.0",
        Title = "HelloID Access Control target system example API",
        Description = "This example API specifies example calls for developing a new API for an Access Control System that will be used for user provisioning from HelloID." +
        "<br>Our Starting point, we grant permissions to user accounts and the permissions assigned to a user will be applied to the accessKeys assigned to that user.</br>" +
        "<br>See the Github repo linked below to download the source code.</br>",
        Contact = new OpenApiContact() { Name = "Tools4everBV Github", Url = new Uri("https://github.com/orgs/Tools4everBV/") }
    });

    // The XML is where all code comments are stored and is used to display information in the swagger interface and yaml.
    // Make sure to enable XML documentation file in project settings.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(config =>
{
    // Instead of the default swagger JSON, we need the yaml file for viewing on the web.
    config.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Access-Control-Example-Target-API");
    config.EnableDeepLinking();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
