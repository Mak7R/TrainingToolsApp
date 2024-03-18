using System.Reflection;
using Contracts.Client.Services;
using Microsoft.Extensions.Configuration;
using Services.Client;
using TrainingToolsApp.Services;

namespace TrainingToolsApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });
        
        
        var a = Assembly.GetExecutingAssembly();
        using (var stream = a.GetManifestResourceStream("TrainingToolsApp.appsettings.json"))
        {
            if (stream == null) throw new NullReferenceException("Stream was null");
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
            
            builder.Configuration.AddConfiguration(config);
        }
        
        builder.Services.AddMauiBlazorWebView();
        
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IFetchService, CrossPlatformFetchService>();
        builder.Services.AddScoped<ICookiesProvider, MauiCookiesProvider>();
        builder.Services.AddSingleton<ILinkGenerator, ConfigLinkGenerator>();
        
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}