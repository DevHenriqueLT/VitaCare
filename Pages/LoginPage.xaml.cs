using VitaCare.Services;
using VitaCare.Models;
using Microsoft.Maui.Storage;

namespace VitaCare.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly UsuarioService _usuarioService;

        public LoginPage()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string nome = txtUsuario.Text?.Trim() ?? "";
            string senha = txtSenha.Text ?? "";

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(senha))
            {
                await DisplayAlert("Atenção", "Preencha usuário e senha.", "OK");
                return;
            }

            var usuario = await _usuarioService.ValidarLoginAsync(nome, senha);

            if (usuario != null)
            {
                await SecureStorage.Default.SetAsync("user_id", usuario.Id.ToString());
                Application.Current.MainPage = new AppShell(); // Redireciona para a página principal
            }
            else
            {
                await DisplayAlert("Erro", "Usuário ou senha inválidos.", "OK");
            }
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsuarioPage());
        }
    }
}
