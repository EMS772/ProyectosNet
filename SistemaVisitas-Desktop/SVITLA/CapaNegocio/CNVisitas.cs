using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNVisitas
    {
        private CDVisitas OBJU=new CDVisitas();

        public void insertar(string nombreAula, string nombreEdificio, string nombreVisitante, string apellidoVisitante, string carrera, string correo, DateTime horaEntrada, DateTime horaSalida, string motivoVisita)
        {
            OBJU.RegistrarV(nombreAula ,nombreEdificio, nombreVisitante, apellidoVisitante, carrera, correo, horaEntrada, horaSalida, motivoVisita);
        }
        public DataTable Cargar()
        {
            DataTable dt = new DataTable();
            dt = OBJU.Visitas();
            return dt;
        }
    }
}
