using System.ComponentModel.DataAnnotations.Schema;

namespace INTEGRACION_SANJUAN.Models
{
    [Table("Curso")]
    public class Curso
    {
        public int Id { get; set; }
        public string curso { get; set; }
        public int creditos { get; set; }
        public int horasSemanal { get; set; }
        public string ciclo { get; set; }

        // Relación
        public int? IdDocente { get; set; }
        public Docente? Docente { get; set; }
    }
}
