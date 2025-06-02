using VitaCare.Pages;

namespace VitaCare;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnUsuariosClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UsuarioListPage");
    }

    private async void OnConsultasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ConsultaMedicaListPage");
    }

    private async void OnMedicamentosClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MedicamentoListPage");
    }

    private async void OnEnfermidadesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///EnfermidadeListPage");
    }

    private void OnSairClicked(object sender, EventArgs e)
    {
        SecureStorage.Default.Remove("user_id");
        Application.Current.Windows[0].Page = new LoginPage();
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#elif IOS
        // iOS não permite fechar o app programaticamente.
#endif
    }
}
