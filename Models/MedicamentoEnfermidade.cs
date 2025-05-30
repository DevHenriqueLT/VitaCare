using SQLite;

namespace VitaCare.Models
{
    [Table("Tb_MedicamentoEnfermidade")]
    public class MedicamentoEnfermidade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int MedicamentoId { get; set; }

        [NotNull]
        public int EnfermidadeId { get; set; }
    }
}
