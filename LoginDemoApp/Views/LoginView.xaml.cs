using LoginDemoApp.ViewModels;
using LoginDemoApp.Services;

namespace LoginDemoApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
		
	}
}