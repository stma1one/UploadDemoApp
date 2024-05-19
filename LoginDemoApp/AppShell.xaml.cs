using LoginDemoApp.Views;

namespace LoginDemoApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("Profile", typeof(ProfilePage));
	}
}
