using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    public partial class UsuarioPage : ContentPage
    {
        private readonly UsuarioService _usuarioService;
        private int? _usuarioIdEdicao = null;        

        public UsuarioPage(int? usuarioId = null)
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            _usuarioIdEdicao = usuarioId;
            if (_usuarioIdEdicao.HasValue)
                CarregarUsuarioParaEdicao(_usuarioIdEdicao.Value);
        }

        private async void CarregarUsuarioParaEdicao(int userId)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(userId);
            if (usuario != null)
            {
                cpfEntry.Text = usuario.Cpf;
                nomeEntry.Text = usuario.Nome;
                emailEntry.Text = usuario.Email;
                senhaEntry.Text = usuario.Senha;
                cpfEntry.IsEnabled = false; // CPF não deve ser editável
            }
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

            if (_usuarioIdEdicao.HasValue)
            {
                // Edição de usuário existente
                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(_usuarioIdEdicao.Value);
                if (usuario != null)
                {
                    usuario.Nome = nomeEntry.Text;
                    usuario.Email = emailEntry.Text;
                    usuario.Senha = senhaEntry.Text;
                    await _usuarioService.AtualizarUsuarioAsync(usuario);
                    await DisplayAlert("Sucesso", "Dados atualizados!", "OK");
                }
            }
            else
            {
                // Cadastro de novo usuário
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

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
