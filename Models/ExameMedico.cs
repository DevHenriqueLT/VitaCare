using SQLite;
using System;

namespace VitaCare.Models
{
    [Table("Tb_ExameMedico")]
    public class ExameMedico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Tipo { get; set; }

        public DateTime? Data { get; set; }

        [MaxLength(200)]
        public string CaminhoArquivo { get; set; }

        [MaxLength(30)]
        public string Observacoes { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
