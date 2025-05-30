using SQLite;

namespace VitaCare.Models
{
    [Table("Tb_Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(14), NotNull]
        public string Cpf { get; set; }

        [MaxLength(30), NotNull]
        public string Nome { get; set; }

        [MaxLength(20), NotNull]
        public string Email { get; set; }

        [MaxLength(10), NotNull]
        public string Senha { get; set; }
    }
}
