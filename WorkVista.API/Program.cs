using WorkVista.API.ExtensionClasses;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.ModelLayer.AppSettings;


// **********************************************
// Create a WebApplicationBuilder object
// to configure the how the ASP.NET service runs
// **********************************************
var builder = WebApplication.CreateBuilder(args);

// **********************************************
// Add and Configure Services
// **********************************************
// Add & Configure Global Application Settings
builder.ConfigureGlobalSettings();

// Add & Configure Interface and Classes
builder.Services.AddInterfaceClasses();

// Add & Configure CORS
builder.Services.ConfigureCors(builder.Configuration.GetRequiredSection("AppSettings").Get<AppSettingsModel>());


// Add & Configure Logging using Serilog
builder.Host.ConfigureSeriLog();

// Add & Configure JWT Authentication
builder.Services.ConfigureJwtAuthentication(
  builder.Configuration.GetRequiredSection("AppSettings").Get<AppSettingsModel>());

// Add & Configure JWT Authorization
builder.Services.ConfigureJwtAuthorization();

// Configure ASP.NET to use the Controller model
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();


// Add & Configure Open API (Swagger)
builder.Services.ConfigureOpenAPI();

// **********************************************
// After adding and configuring services
// Create an instance of a WebApplication object
// **********************************************
var app = builder.Build();

//app.UseCors("AllowSpecificOrigin");
// **********************************************
// Configure the HTTP Request Pipeline
// **********************************************
if (app.Environment.IsDevelopment())
{
    // When in Development mode
    // Enable the Open API (Swagger) page
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "swagger/{documentName}.json";
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/swagger.json", "WorkVista API");
    });
}


// Enable Exception Handling Middleware
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/DevelopmentError");
}
else
{
    app.UseExceptionHandler("/ProductionError");
}
app.UseDeveloperExceptionPage();
// Handle status code errors in the range 400-599
app.UseStatusCodePagesWithReExecute("/StatusCodeHandler/{0}");

// Enable CORS Middleware
app.UseCors(CommonConstants.CORS_POLICY);
//app.UseCors();

//app.UseHttpsRedirection(); dont us this in API
// Enable Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();
// Enable the endpoints of Controller Action Methods
app.MapControllers();


// Run the Application
app.Run();
