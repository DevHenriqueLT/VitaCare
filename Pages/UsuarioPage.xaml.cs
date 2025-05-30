using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    public partial class UsuarioPage : ContentPage
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioPage()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cpfEntry.Text) ||
                string.IsNullOrWhiteSpace(nomeEntry.Text) ||
                string.IsNullOrWhiteSpace(emailEntry.Text) ||
                string.IsNullOrWhiteSpace(senhaEntry.Text))
            {
                await DisplayAlert("Erro", "Todos os campos são obrigatórios.", "OK");
                return;
            }

            var novoUsuario = new Usuario
            {
                Cpf = cpfEntry.Text,
                Nome = nomeEntry.Text,
                Email = emailEntry.Text,
                Senha = senhaEntry.Text
            };

            try
            {
                await _usuarioService.AdicionarUsuarioAsync(novoUsuario);
                await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");

                cpfEntry.Text = nomeEntry.Text = emailEntry.Text = senhaEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar: {ex.Message}", "OK");
            }
        }
    }
}
