using VitaCare.Models;
using VitaCare.Services;
using Microsoft.Maui.Storage;

namespace VitaCare.Pages
{
    public partial class EnfermidadeListPage : ContentPage
    {
        private readonly EnfermidadeService _enfermidadeService;
        private List<Enfermidade> _todasEnfermidades;

        public EnfermidadeListPage()
        {
            InitializeComponent();
            _enfermidadeService = new EnfermidadeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarEnfermidades();
        }

        private async Task CarregarEnfermidades()
        {
            var userIdStr = await SecureStorage.Default.GetAsync("user_id");
            if (int.TryParse(userIdStr, out int userId))
            {
                _todasEnfermidades = await _enfermidadeService.ObterEnfermidadesPorUsuarioAsync(userId);
                enfermidadesCollectionView.ItemsSource = _todasEnfermidades;
            }
            else
            {
                await DisplayAlert("Erro", "Usu�rio n�o autenticado.", "OK");
                // Opcional: redirecionar para login
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var termo = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(termo))
            {
                enfermidadesCollectionView.ItemsSource = _todasEnfermidades;
            }
            else
            {
                var filtradas = _todasEnfermidades.Where(e =>
                    (e.Nome?.ToLower().Contains(termo) ?? false) ||
                    (e.Observacao?.ToLower().Contains(termo) ?? false)).ToList();

                enfermidadesCollectionView.ItemsSource = filtradas;
            }
        }

        private async void OnEditarClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var enfermidade = (Enfermidade)button.CommandParameter;

            await Navigation.PushAsync(new EnfermidadePage(enfermidade));
        }

        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var enfermidade = (Enfermidade)button.CommandParameter;

            var confirmar = await DisplayAlert("Confirma��o",
                $"Deseja excluir \"{enfermidade.Nome}\"?",
                "Sim", "Cancelar");

            if (confirmar)
            {
                await _enfermidadeService.RemoverEnfermidadeAsync(enfermidade);
                await DisplayAlert("Removido", "Enfermidade exclu�da com sucesso.", "OK");
                await CarregarEnfermidades();
            }
        }
    }
}
