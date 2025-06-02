using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    public partial class MedicamentoPage : ContentPage
    {
        private readonly MedicamentoService _medicamentoService;
        private Medicamento _medicamentoAtual;
        private bool _modoEdicao;
        private readonly EnfermidadeService _enfermidadeService;
        private readonly MedicamentoEnfermidadeService _associacaoService;
        private List<EnfermidadeViewModel> _enfermidadesVM;
        private int? _ultimaEnfermidadeCadastradaId;



        public MedicamentoPage(Medicamento? medicamento = null)
        {
            InitializeComponent();
            _medicamentoService = new MedicamentoService();
            _enfermidadeService = new EnfermidadeService();
            _associacaoService = new MedicamentoEnfermidadeService();
            CarregarEnfermidadesAsync();

            if (medicamento != null)
            {
                _modoEdicao = true;
                _medicamentoAtual = medicamento;

                nomeEntry.Text = medicamento.Nome;
                dosagemEntry.Text = medicamento.Dosagem;
                frequenciaEntry.Text = medicamento.Frequencia;
            }
            else
            {
                _modoEdicao = false;
                _medicamentoAtual = new Medicamento();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var todas = await _enfermidadeService.ObterTodasEnfermidadesAsync();
            var ultima = todas.OrderByDescending(e => e.Id).FirstOrDefault();

            if (ultima != null && (_enfermidadesVM == null || !_enfermidadesVM.Any(e => e.Id == ultima.Id)))
            {
                _ultimaEnfermidadeCadastradaId = ultima.Id;
            }

            CarregarEnfermidadesAsync();
        }


        private async void CarregarEnfermidadesAsync()
        {
            var todasEnfermidades = await _enfermidadeService.ObterTodasEnfermidadesAsync();
            var associadas = new List<MedicamentoEnfermidade>();

            if (_modoEdicao)
            {
                associadas = await _associacaoService.ObterAssociacoesPorMedicamentoAsync(_medicamentoAtual.Id);
            }

            _enfermidadesVM = todasEnfermidades.Select(e => new EnfermidadeViewModel
            {
                Id = e.Id,
                Nome = e.Nome,
                Selecionado = associadas.Any(a => a.EnfermidadeId == e.Id) ||
                              (_ultimaEnfermidadeCadastradaId != null && e.Id == _ultimaEnfermidadeCadastradaId)
            }).ToList();

            enfermidadesCollectionView.ItemsSource = null;
            enfermidadesCollectionView.ItemsSource = _enfermidadesVM;
        }

        private async void OnNovaEnfermidadeClicked(object sender, EventArgs e)
        {
            var pagina = new EnfermidadePage();
            await Navigation.PushAsync(pagina);
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomeEntry.Text) ||
                string.IsNullOrWhiteSpace(dosagemEntry.Text) ||
                string.IsNullOrWhiteSpace(frequenciaEntry.Text))
            {
                await DisplayAlert("Erro", "Todos os campos são obrigatórios.", "OK");
                return;
            }

            // Recupera o id do usuário autenticado
            var userIdStr = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync("user_id");
            if (!int.TryParse(userIdStr, out int userId))
            {
                await DisplayAlert("Erro", "Usuário não autenticado.", "OK");
                return;
            }

            _medicamentoAtual.Nome = nomeEntry.Text.Trim();
            _medicamentoAtual.Dosagem = dosagemEntry.Text.Trim();
            _medicamentoAtual.Frequencia = frequenciaEntry.Text.Trim();
            _medicamentoAtual.UsuarioId = userId; // <-- Atribuição correta do usuário

            try
            {
                if (_modoEdicao)
                {
                    await _medicamentoService.AtualizarMedicamentoAsync(_medicamentoAtual);
                }
                else
                {
                    await _medicamentoService.AdicionarMedicamentoAsync(_medicamentoAtual);
                }

                // Gerenciar associações com enfermidades
                var selecionadas = _enfermidadesVM.Where(e => e.Selecionado).ToList();

                if (_modoEdicao)
                {
                    var anteriores = await _associacaoService.ObterAssociacoesPorMedicamentoAsync(_medicamentoAtual.Id);
                    foreach (var assoc in anteriores)
                    {
                        await _associacaoService.RemoverAssociacaoAsync(assoc);
                    }
                }

                foreach (var enf in selecionadas)
                {
                    await _associacaoService.AdicionarAssociacaoAsync(new MedicamentoEnfermidade
                    {
                        MedicamentoId = _medicamentoAtual.Id,
                        EnfermidadeId = enf.Id
                    });
                }

                string mensagem = _modoEdicao ? "Medicamento atualizado com sucesso!" : "Medicamento cadastrado com sucesso!";
                await DisplayAlert("Sucesso", mensagem, "OK");

                if (!_modoEdicao)
                {
                    nomeEntry.Text = dosagemEntry.Text = frequenciaEntry.Text = string.Empty;

                    // Desmarcar checkboxes
                    foreach (var enf in _enfermidadesVM)
                        enf.Selecionado = false;

                    enfermidadesCollectionView.ItemsSource = null;
                    enfermidadesCollectionView.ItemsSource = _enfermidadesVM;
                }

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar: {ex.Message}", "OK");
            }
        }
    }
}
