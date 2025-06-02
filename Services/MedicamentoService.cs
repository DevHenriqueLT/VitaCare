using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using VitaCare.Data;
using VitaCare.Models;

namespace VitaCare.Services
{
    public class MedicamentoService
    {
        private readonly SQLiteAsyncConnection _db;

        public MedicamentoService()
        {
            _db = DatabaseContext.Database;
        }

        public async Task<int> AdicionarMedicamentoAsync(Medicamento medicamento)
        {
            return await _db.InsertAsync(medicamento);
        }

        public async Task<List<Medicamento>> ObterTodosMedicamentosAsync()
        {
            return await _db.Table<Medicamento>().ToListAsync();
        }

        public async Task<List<Medicamento>> ObterMedicamentosPorUsuarioAsync(int usuarioId)
        {
            return await _db.Table<Medicamento>()
                .Where(m => m.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<int> AtualizarMedicamentoAsync(Medicamento medicamento)
        {
            return await _db.UpdateAsync(medicamento);
        }

        public async Task<int> RemoverMedicamentoAsync(Medicamento medicamento)
        {
            return await _db.DeleteAsync(medicamento);
        }
    }
}
