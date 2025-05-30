using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using VitaCare.Data;
using VitaCare.Models;

namespace VitaCare.Services
{
    public class ConsultaMedicaService
    {
        private readonly SQLiteAsyncConnection _db;

        public ConsultaMedicaService()
        {
            _db = DatabaseContext.Database;
        }

        public async Task<int> AdicionarConsultaAsync(ConsultaMedica consulta)
        {
            return await _db.InsertAsync(consulta);
        }

        public async Task<List<ConsultaMedica>> ObterTodasConsultasAsync()
        {
            return await _db.Table<ConsultaMedica>().ToListAsync();
        }

        public async Task<ConsultaMedica> ObterConsultaPorIdAsync(int id)
        {
            return await _db.Table<ConsultaMedica>()
                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> AtualizarConsultaAsync(ConsultaMedica consulta)
        {
            return await _db.UpdateAsync(consulta);
        }

        public async Task<int> RemoverConsultaAsync(ConsultaMedica consulta)
        {
            return await _db.DeleteAsync(consulta);
        }
    }
}
