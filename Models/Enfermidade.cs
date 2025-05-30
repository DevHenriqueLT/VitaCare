using SQLite;

namespace VitaCare.Models
{
    [Table("Tb_Enfermidade")]
    public class Enfermidade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(20), NotNull]
        public string Nome { get; set; }

        [MaxLength(50)]
        public string Observacao { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
