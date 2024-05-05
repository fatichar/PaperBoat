namespace App.ViewModels;

public class MainViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to PaperBoat!";
#pragma warning restore CA1822 // Mark members as static
}