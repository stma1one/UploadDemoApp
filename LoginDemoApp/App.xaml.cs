using LoginDemoApp.Models;
using LoginDemoApp.ViewModels;
using LoginDemoApp.Views;
namespace LoginDemoApp;

public partial class App : Application
{
	
	public App()
	{
		
		InitializeComponent();

		MainPage = new AppShell();
	}
}
