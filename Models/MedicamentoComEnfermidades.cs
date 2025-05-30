using System.Collections.Generic;

namespace VitaCare.Models
{
    public class MedicamentoComEnfermidades
    {
        public Medicamento Medicamento { get; set; }
        public List<string> Enfermidades { get; set; }

        public string EnfermidadesFormatadas =>
            Enfermidades != null && Enfermidades.Any()
                ? string.Join(", ", Enfermidades)
                : "Nenhuma enfermidade associada.";
    }
}
