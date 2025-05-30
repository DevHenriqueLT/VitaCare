using VitaCare.Services;
using VitaCare.Models;

namespace VitaCare.Pages
{
    public partial class UsuarioListPage : ContentPage
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioListPage()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
        }

        private async Task RecarregarLista()
        {
            try
            {
                var usuarios = await _usuarioService.ObterTodosUsuariosAsync();
                usuariosCollectionView.ItemsSource = usuarios;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao carregar dados: {ex.Message}", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RecarregarLista();
        }

        private async void OnExcluirUsuarioClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var usuario = (Usuario)button.CommandParameter;

            var confirmar = await DisplayAlert("Confirmação",
                $"Deseja realmente excluir {usuario.Nome}?",
                "Sim", "Cancelar");

            if (confirmar)
            {
                await _usuarioService.RemoverUsuarioAsync(usuario);
                await DisplayAlert("Sucesso", "Usuário excluído.", "OK");
                await RecarregarLista();
            }
        }

        private async void OnEditarUsuarioClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var usuario = (Usuario)button.CommandParameter;

            var novoNome = await DisplayPromptAsync("Editar Nome", "Digite o novo nome:", initialValue: usuario.Nome);

            if (!string.IsNullOrWhiteSpace(novoNome) && novoNome != usuario.Nome)
            {
                usuario.Nome = novoNome;
                await _usuarioService.AtualizarUsuarioAsync(usuario);
                await DisplayAlert("Sucesso", "Usuário atualizado.", "OK");
                await RecarregarLista();
            }
        }
    }
}
