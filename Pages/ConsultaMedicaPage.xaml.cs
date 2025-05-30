using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    public partial class ConsultaMedicaPage : ContentPage
    {
        private readonly ConsultaMedicaService _consultaService;
        private ConsultaMedica _consultaAtual;
        private bool _modoEdicao;

        public ConsultaMedicaPage(ConsultaMedica? consulta = null)
        {
            InitializeComponent();
            _consultaService = new ConsultaMedicaService();

            if (consulta != null)
            {
                _consultaAtual = consulta;
                _modoEdicao = true;

                dataPicker.Date = consulta.Data;
                horaPicker.Time = consulta.Hora;
                medicoEntry.Text = consulta.Medico;
                localEntry.Text = consulta.Local;
                statusPicker.SelectedItem = consulta.Status;
            }
            else
            {
                _consultaAtual = new ConsultaMedica();
                _modoEdicao = false;
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(medicoEntry.Text) ||
                string.IsNullOrWhiteSpace(localEntry.Text) ||
                statusPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Erro", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }

            _consultaAtual.Data = dataPicker.Date;
            _consultaAtual.Hora = horaPicker.Time;
            _consultaAtual.Medico = medicoEntry.Text;
            _consultaAtual.Local = localEntry.Text;
            _consultaAtual.Status = statusPicker.SelectedItem.ToString();
            _consultaAtual.UsuarioId = 1; // Fixo para testes

            try
            {
                if (_modoEdicao)
                {
                    await _consultaService.AtualizarConsultaAsync(_consultaAtual);
                    await DisplayAlert("Atualizado", "Consulta atualizada com sucesso.", "OK");
                }
                else
                {
                    await _consultaService.AdicionarConsultaAsync(_consultaAtual);
                    await DisplayAlert("Salvo", "Consulta cadastrada com sucesso!", "OK");

                    medicoEntry.Text = localEntry.Text = string.Empty;
                    statusPicker.SelectedIndex = -1;
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
