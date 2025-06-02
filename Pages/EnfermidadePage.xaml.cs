using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    public partial class EnfermidadePage : ContentPage
    {
        private readonly EnfermidadeService _enfermidadeService;
        private Enfermidade _enfermidadeAtual;
        private bool _modoEdicao;

        public EnfermidadePage(Enfermidade? enfermidade = null)
        {
            InitializeComponent();
            _enfermidadeService = new EnfermidadeService();

            if (enfermidade != null)
            {
                _modoEdicao = true;
                _enfermidadeAtual = enfermidade;

                nomeEntry.Text = enfermidade.Nome;
                observacaoEntry.Text = enfermidade.Observacao;
            }
            else
            {
                _modoEdicao = false;
                _enfermidadeAtual = new Enfermidade();
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomeEntry.Text))
            {
                await DisplayAlert("Erro", "O campo 'Nome' � obrigat�rio.", "OK");
                return;
            }

            // Recupera o id do usu�rio autenticado
            var userIdStr = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync("user_id");
            if (!int.TryParse(userIdStr, out int userId))
            {
                await DisplayAlert("Erro", "Usu�rio n�o autenticado.", "OK");
                return;
            }

            _enfermidadeAtual.Nome = nomeEntry.Text.Trim();
            _enfermidadeAtual.Observacao = observacaoEntry.Text?.Trim();
            _enfermidadeAtual.UsuarioId = userId; // <-- Atribui��o correta

            try
            {
                if (_modoEdicao)
                {
                    await _enfermidadeService.AtualizarEnfermidadeAsync(_enfermidadeAtual);
                    await DisplayAlert("Atualizado", "Enfermidade atualizada com sucesso!", "OK");
                }
                else
                {
                    await _enfermidadeService.AdicionarEnfermidadeAsync(_enfermidadeAtual);
                    await DisplayAlert("Salvo", "Enfermidade cadastrada com sucesso!", "OK");

                    nomeEntry.Text = observacaoEntry.Text = string.Empty;
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
