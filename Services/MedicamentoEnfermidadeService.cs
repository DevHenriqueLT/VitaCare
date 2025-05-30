using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using VitaCare.Data;
using VitaCare.Models;

namespace VitaCare.Services
{
    public class MedicamentoEnfermidadeService
    {
        private readonly SQLiteAsyncConnection _db;

        public MedicamentoEnfermidadeService()
        {
            _db = DatabaseContext.Database;
        }

        public async Task<int> AdicionarAssociacaoAsync(MedicamentoEnfermidade associacao)
        {
            return await _db.InsertAsync(associacao);
        }

        public async Task<List<MedicamentoEnfermidade>> ObterTodasAssociacoesAsync()
        {
            return await _db.Table<MedicamentoEnfermidade>().ToListAsync();
        }

        public async Task<List<MedicamentoEnfermidade>> ObterAssociacoesPorMedicamentoAsync(int medicamentoId)
        {
            return await _db.Table<MedicamentoEnfermidade>()
                            .Where(a => a.MedicamentoId == medicamentoId)
                            .ToListAsync();
        }

        public async Task<List<MedicamentoEnfermidade>> ObterAssociacoesPorEnfermidadeAsync(int enfermidadeId)
        {
            return await _db.Table<MedicamentoEnfermidade>()
                            .Where(a => a.EnfermidadeId == enfermidadeId)
                            .ToListAsync();
        }

        public async Task<int> RemoverAssociacaoAsync(MedicamentoEnfermidade associacao)
        {
            return await _db.DeleteAsync(associacao);
        }
    }
}
