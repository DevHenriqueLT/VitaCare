using VitaCare.Models;
using VitaCare.Services;

namespace VitaCare.Pages
{
    /*public partial class ConsultaMedicaListPage : ContentPage
    {
        private readonly ConsultaMedicaService _consultaService;

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
            var consultas = await _consultaService.ObterTodasConsultasAsync();
            consultasCollectionView.ItemsSource = consultas;
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
                await DisplayAlert("Removido", "Consulta exclu�da com sucesso.", "OK");
                await RecarregarConsultas();
            }
        }
    }*/
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
            _todasConsultas = await _consultaService.ObterTodasConsultasAsync();
            consultasCollectionView.ItemsSource = _todasConsultas;
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
                await DisplayAlert("Removido", "Consulta exclu�da com sucesso.", "OK");
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
    }
}
