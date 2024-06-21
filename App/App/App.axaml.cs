using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using App.ViewModels;
using App.Views;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace App;

public partial class App : Application
{
    private IServiceProvider Services { get; } = ConfigureServices();

    private static IServiceProvider ConfigureServices()
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(envName) && !IsInDesignMode())
        {
            throw new Exception("ASPNETCORE_ENVIRONMENT environment variable is not set.");
        }

        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{envName}.json", optional: true)
            .Build();

        var config = new AppConfig();
        configuration.Bind(config);

        var services = new ServiceCollection();
        services.AddSingleton(config);

        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var myDataPath = Path.Combine(appDataPath, "PaperBoat");
        // Create the directory if it doesn't exist
        Directory.CreateDirectory(myDataPath);

        services.AddSingleton<MainViewModel>();

        services.AddLogging();
        ConfigureLogging();

        return services.BuildServiceProvider();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("The global logger has been configured");
        System.Diagnostics.Debug.WriteLine("Log to console");
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var mainViewModel = Services.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static bool IsInDesignMode()
    {
        return AppDomain.CurrentDomain.FriendlyName.Contains("Designer");
    }
}