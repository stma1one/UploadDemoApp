using LoginDemoApp.Services;
using LoginDemoApp.ViewModels;
using LoginDemoApp.Views;
using Microsoft.Extensions.Logging;

namespace LoginDemoApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<LoginView>();
		builder.Services.AddSingleton<LoginDemoWebAPIProxy>();	
		builder.Services.AddSingleton<ProfilePageViewModel>();
        builder.Services.AddSingleton<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
