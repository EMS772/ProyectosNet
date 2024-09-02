using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNAulas
    {   
       private CDAulas ObjAulas = new CDAulas();

        public DataTable Aulas()
        {
            DataTable dt = new DataTable();
            dt = ObjAulas.ListarAulas();
            return dt;
        }

        public void Eliminar(string ID)
        {
            ObjAulas.EliminarAulas(int.Parse(ID));
        }

        public void InsertarA(string NombreAulas)
        {
            ObjAulas.InsertarAulas(NombreAulas);
        }

        public void ModificarA(string IdAula, string NombreAula)
        {
            ObjAulas.Modificar(int.Parse(IdAula), NombreAula);
        }
    }
}
