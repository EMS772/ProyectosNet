using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNUsuario
    {
        private CDUsuario objCDusuarios = new CDUsuario();
        public List<Usuarios> Listar()
        {
            return objCDusuarios.Listar();
        }
        public DataTable MostrarU()
        {
            DataTable dt = new DataTable();
            dt = objCDusuarios.MostrarUs();
            return dt;
        }

        public void Insertaru(string Nombre, string Apellido, DateTime FechaN, string TipoUsuario, string Usuario, string Contraseña)
        {
            objCDusuarios.InsertarU(Nombre, Apellido, FechaN, TipoUsuario, Usuario, Contraseña);
        }
        public void Eliminar(string ID)
        {
            objCDusuarios.Eliminar(int.Parse(ID));
        }

        public void Modificar(string idUsuario, string Nombre, string Apellido, DateTime FechaN, string TipoUsuario, string Usuario, string Contraseña)
        {
            objCDusuarios.Modificar(int.Parse(idUsuario), Nombre, Apellido, FechaN, TipoUsuario, Usuario, Contraseña);
        }
    }
}
