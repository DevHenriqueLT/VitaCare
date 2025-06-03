using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{    
    public partial class ConsultaMedicaListPage : ContentPage
    {
        private readonly ConsultaMedicaService _consultaService;
        private List<ConsultaMedica> _todasConsultas;

        public ConsultaMedicaListPage()
        {
            InitializeComponent();
            _consultaService = new ConsultaMedicaService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RecarregarConsultas();
        }

        private async Task RecarregarConsultas()
        {
            var userIdStr = await SecureStorage.Default.GetAsync("user_id");
            if (int.TryParse(userIdStr, out int userId))
            {
                _todasConsultas = await _consultaService.ObterConsultasPorUsuarioAsync(userId);
                consultasCollectionView.ItemsSource = _todasConsultas;
            }
            else
            {
                await DisplayAlert("Erro", "Usuário não autenticado.", "OK");
                // Opcional: redirecionar para login
            }
        }

        private async void OnEditarConsultaClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var consulta = (ConsultaMedica)button.CommandParameter;

            await Navigation.PushAsync(new ConsultaMedicaPage(consulta));
        }

        private async void OnExcluirConsultaClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var consulta = (ConsultaMedica)button.CommandParameter;

            var confirmar = await DisplayAlert("Confirmar",
                $"Deseja realmente excluir a consulta com {consulta.Medico} em {consulta.Data:dd/MM/yyyy}?",
                "Sim", "Cancelar");

            if (confirmar)
            {
                await _consultaService.RemoverConsultaAsync(consulta);
                await DisplayAlert("Removido", "Consulta excluída com sucesso.", "OK");
                await RecarregarConsultas();
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var termo = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(termo))
            {
                consultasCollectionView.ItemsSource = _todasConsultas;
            }
            else
            {
                var filtradas = _todasConsultas.Where(c =>
                    (c.Medico?.ToLower().Contains(termo) ?? false) ||
                    (c.Local?.ToLower().Contains(termo) ?? false) ||
                    (c.Status?.ToLower().Contains(termo) ?? false)).ToList();

                consultasCollectionView.ItemsSource = filtradas;
            }
        }

        private async void OnAdicionarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConsultaMedicaPage());
        }

        private void OnVoltarInicioClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//main");
        }
    }
}
