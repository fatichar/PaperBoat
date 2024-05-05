using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using App;

[assembly: SupportedOSPlatform("browser")]

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildApp()
        .WithInterFont()
        .UseReactiveUI()
        .StartBrowserAppAsync("out");

    public static AppBuilder BuildApp()
        => AppBuilder.Configure<App.App>();
}