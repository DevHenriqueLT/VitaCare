using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    public partial class MedicamentoListPage : ContentPage
    {
        private readonly MedicamentoService _medicamentoService;
        private readonly MedicamentoEnfermidadeService _associacaoService;
        private readonly EnfermidadeService _enfermidadeService;

        private List<MedicamentoComEnfermidades> _todosMedicamentos;

        public MedicamentoListPage()
        {
            InitializeComponent();
            _medicamentoService = new MedicamentoService();
            _associacaoService = new MedicamentoEnfermidadeService();
            _enfermidadeService = new EnfermidadeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RecarregarMedicamentos();
        }

        private async Task RecarregarMedicamentos()
        {
            var medicamentos = await _medicamentoService.ObterTodosMedicamentosAsync();
            var associacoes = await _associacaoService.ObterTodasAssociacoesAsync();
            var enfermidades = await _enfermidadeService.ObterTodasEnfermidadesAsync();

            _todosMedicamentos = medicamentos.Select(m => new MedicamentoComEnfermidades
            {
                Medicamento = m,
                Enfermidades = associacoes
                    .Where(a => a.MedicamentoId == m.Id)
                    .Select(a =>
                        enfermidades.FirstOrDefault(e => e.Id == a.EnfermidadeId)?.Nome ?? "")
                    .Where(n => !string.IsNullOrEmpty(n))
                    .ToList()
            }).ToList();

            medicamentosCollectionView.ItemsSource = _todosMedicamentos;
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var termo = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(termo))
            {
                medicamentosCollectionView.ItemsSource = _todosMedicamentos;
            }
            else
            {
                var filtrados = _todosMedicamentos.Where(m =>
                    m.Medicamento.Nome?.ToLower().Contains(termo) == true ||
                    m.Medicamento.Frequencia?.ToLower().Contains(termo) == true ||
                    m.Enfermidades.Any(e => e.ToLower().Contains(termo))
                ).ToList();

                medicamentosCollectionView.ItemsSource = filtrados;
            }
        }

        private async void OnEditarMedicamentoClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (MedicamentoComEnfermidades)button.CommandParameter;

            await Navigation.PushAsync(new MedicamentoPage(item.Medicamento));
        }

        private async void OnExcluirMedicamentoClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (MedicamentoComEnfermidades)button.CommandParameter;

            var confirmar = await DisplayAlert("Confirmar",
                $"Deseja realmente excluir \"{item.Medicamento.Nome}\"?",
                "Sim", "Cancelar");

            if (confirmar)
            {
                await _medicamentoService.RemoverMedicamentoAsync(item.Medicamento);

                var associacoes = await _associacaoService.ObterAssociacoesPorMedicamentoAsync(item.Medicamento.Id);
                foreach (var assoc in associacoes)
                {
                    await _associacaoService.RemoverAssociacaoAsync(assoc);
                }

                await DisplayAlert("Removido", "Medicamento excluído com sucesso.", "OK");
                await RecarregarMedicamentos();
            }
        }
    }
}
