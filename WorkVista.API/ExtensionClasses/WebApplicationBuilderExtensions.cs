using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.ModelLayer.AppSettings;

namespace WorkVista.API.ExtensionClasses
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureGlobalSettings(this WebApplicationBuilder builder)
        {
            // Configure Global Settings

            // Read "AppSettingsModel" section
            // Use the IOptionsMonitor<AppSettingsModel> in controller's constructor
            builder.Services.Configure<AppSettingsModel>(builder.Configuration.GetSection("AppSettings"));

            // NOTE: The following lines are only used for the ConfigTestController
            builder.Services.AddSingleton<AppSettingsModel, AppSettingsModel>();
            // Read "AppSettingsModel" section and add as a singleton
            AppSettingsModel settings = new();
            builder.Configuration.GetSection(CommonConstants.CORS_POLICY).Bind(settings);
            builder.Services.AddSingleton<AppSettingsModel>(settings);
        }
    }
}
