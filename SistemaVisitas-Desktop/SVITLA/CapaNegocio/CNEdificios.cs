using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNEdificios
    {
        private CDEdificios objCDEdificio = new CDEdificios();

        public DataTable MostrarE()
        {
            DataTable dt = new DataTable();
            dt = objCDEdificio.Mostrar();
            return dt;
        }

        public void InsertarE(string NombreEdificio)
        {
           objCDEdificio.InsertarE(NombreEdificio);
        }
        public void ModificarA(string idEdificio, string NombreEdificio)
        {
            objCDEdificio.Modificar(int.Parse(idEdificio), NombreEdificio);
        }
        public void Eliminar(string idEdificio)
        {
            objCDEdificio.Eliminar(int.Parse(idEdificio));
        }
    }
}
