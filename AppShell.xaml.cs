using VitaCare.Pages;

namespace VitaCare
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public async void OnLogoutClicked(object sender, EventArgs e)
        {
            SecureStorage.Default.Remove("user_id");
            Application.Current.Windows[0].Page = new LoginPage();
        }

        private async void OnMeuPerfilClicked(object sender, EventArgs e)
        {
            var userIdStr = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync("user_id");
            if (int.TryParse(userIdStr, out int userId))
            {
                await Shell.Current.Navigation.PushAsync(new UsuarioPage(userId));
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Usuário não autenticado.", "OK");
            }
        }

    }
}
