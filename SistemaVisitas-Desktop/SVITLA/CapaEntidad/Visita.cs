using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Visita
    {
        public int IdVisita { get; set; }
        public int IdUsuario { get; set; }
        public int IdAula { get; set; }
        public string NombreVisitante { get; set; }
        public string ApellidoVisitante { get; set; }
        public string Carrera { get; set; }
        public string Correo { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime HoraSalida { get; set; }
        public string MotivoVisita { get; set; }
    }
}
