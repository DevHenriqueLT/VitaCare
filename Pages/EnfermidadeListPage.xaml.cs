using VitaCare.Models;
using VitaCare.Services;

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
            _todasEnfermidades = await _enfermidadeService.ObterTodasEnfermidadesAsync();
            enfermidadesCollectionView.ItemsSource = _todasEnfermidades;
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

            var confirmar = await DisplayAlert("Confirmação",
                $"Deseja excluir \"{enfermidade.Nome}\"?",
                "Sim", "Cancelar");

            if (confirmar)
            {
                await _enfermidadeService.RemoverEnfermidadeAsync(enfermidade);
                await DisplayAlert("Removido", "Enfermidade excluída com sucesso.", "OK");
                await CarregarEnfermidades();
            }
        }
    }
}
