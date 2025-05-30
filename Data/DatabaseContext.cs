using SQLite;
using System.IO;
using System.Threading.Tasks;
using VitaCare.Models;

namespace VitaCare.Data
{
    public class DatabaseContext
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitializeAsync()
        {
            if (_database != null)
                return;

            // Caminho para o arquivo SQLite no dispositivo
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "vitacare.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            // Criação das tabelas
            await _database.CreateTableAsync<Usuario>();
            await _database.CreateTableAsync<Enfermidade>();
            await _database.CreateTableAsync<Medicamento>();
            await _database.CreateTableAsync<ConsultaMedica>();
            await _database.CreateTableAsync<ExameMedico>();
            await _database.CreateTableAsync<MedicamentoEnfermidade>();
        }

        public static SQLiteAsyncConnection Database =>
            _database ?? throw new InvalidOperationException("Database not initialized. Call InitializeAsync() first.");
    }
}
