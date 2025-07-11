using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEGRACION_SANJUAN.Models
{
    [Table("Docente")]
    public class Docente
    {
        public int Id { get; set; }
        public string apellidos { get; set; }
        public string nombres { get; set; }
        public string profesion { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public string correo { get; set; }
    }
}
