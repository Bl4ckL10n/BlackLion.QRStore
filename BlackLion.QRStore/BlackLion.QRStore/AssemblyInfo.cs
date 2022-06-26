using Android.App;
using Xamarin.Forms.Xaml;

#if DEBUG
[assembly: Application(Debuggable=true)]
#else
[assembly: Application(Debuggable = false)]
#endif
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]