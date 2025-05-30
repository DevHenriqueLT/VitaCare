using VitaCare.Data;

namespace VitaCare
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Inicializa o banco de dados de forma assíncrona e segura
            Task.Run(async () =>
            {
                try
                {
                    await DatabaseContext.InitializeAsync();
                }
                catch (Exception ex)
                {
                    // Log ou tratamento de erro pode ser adicionado aqui
                    System.Diagnostics.Debug.WriteLine($"Erro ao inicializar o banco: {ex.Message}");
                }
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
