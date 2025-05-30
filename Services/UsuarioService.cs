using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using VitaCare.Data;
using VitaCare.Models;

namespace VitaCare.Services
{
    public class UsuarioService
    {
        private readonly SQLiteAsyncConnection _db;

        public UsuarioService()
        {
            _db = DatabaseContext.Database;
        }

        public async Task<int> AdicionarUsuarioAsync(Usuario usuario)
        {
            return await _db.InsertAsync(usuario);
        }

        public async Task<List<Usuario>> ObterTodosUsuariosAsync()
        {
            return await _db.Table<Usuario>().ToListAsync();
        }

        public async Task<Usuario> ObterUsuarioPorIdAsync(int id)
        {
            return await _db.Table<Usuario>()
                            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> AtualizarUsuarioAsync(Usuario usuario)
        {
            return await _db.UpdateAsync(usuario);
        }

        public async Task<int> RemoverUsuarioAsync(Usuario usuario)
        {
            return await _db.DeleteAsync(usuario);
        }
    }
}
