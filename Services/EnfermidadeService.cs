﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using VitaCare.Data;
using VitaCare.Models;

namespace VitaCare.Services
{
    public class EnfermidadeService
    {
        private readonly SQLiteAsyncConnection _db;

        public EnfermidadeService()
        {
            _db = DatabaseContext.Database;
        }

        public async Task<int> AdicionarEnfermidadeAsync(Enfermidade enfermidade)
        {
            return await _db.InsertAsync(enfermidade);
        }

        public async Task<List<Enfermidade>> ObterTodasEnfermidadesAsync()
        {
            return await _db.Table<Enfermidade>().ToListAsync();
        }

        public async Task<List<Enfermidade>> ObterEnfermidadesPorUsuarioAsync(int usuarioId)
        {
            return await _db.Table<Enfermidade>()
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<int> AtualizarEnfermidadeAsync(Enfermidade enfermidade)
        {
            return await _db.UpdateAsync(enfermidade);
        }

        public async Task<int> RemoverEnfermidadeAsync(Enfermidade enfermidade)
        {
            return await _db.DeleteAsync(enfermidade);
        }
    }
}
