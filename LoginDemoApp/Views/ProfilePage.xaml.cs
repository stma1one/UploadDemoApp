using LoginDemoApp.ViewModels;

namespace LoginDemoApp.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}