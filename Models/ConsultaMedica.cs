using SQLite;
using System;

namespace VitaCare.Models
{
    [Table("Tb_ConsultaMedica")]
    public class ConsultaMedica
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime Data { get; set; }

        [NotNull]
        public TimeSpan Hora { get; set; }

        [MaxLength(30)]
        public string Medico { get; set; }

        [MaxLength(30)]
        public string Local { get; set; }

        [MaxLength(15)]
        public string Status { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
