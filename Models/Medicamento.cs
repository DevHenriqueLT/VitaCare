using SQLite;
using System;

namespace VitaCare.Models
{
    [Table("Tb_Medicamento")]
    public class Medicamento
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(20), NotNull]
        public string Nome { get; set; }

        [MaxLength(20)]
        public string Dosagem { get; set; }

        [MaxLength(20)]
        public string Frequencia { get; set; }

        [MaxLength(20)]
        public string Horarios { get; set; }

        public DateTime? DataIni { get; set; }

        public DateTime? DataFim { get; set; }

        [MaxLength(50)]
        public string Observacao { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
