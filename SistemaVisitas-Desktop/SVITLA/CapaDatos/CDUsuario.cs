using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDUsuario
    {
        /*#####################################################################################
                                           Funcion Login
             ######################################################################################*/
        public List<Usuarios> Listar()
        {
           List <Usuarios>Lista=new List<Usuarios>();

            try {
                using (SqlConnection ocn = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("ListarUsuarios", ocn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        ocn.Open();
                        using (SqlDataReader leer = cmd.ExecuteReader())
                        {

                            while (leer.Read())
                            {
                                Lista.Add(
                                    new Usuarios()
                                    {
                                        IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
                                        Nombre = leer["Nombre"].ToString(),
                                        Apellido = leer["Apellido"].ToString(),
                                        TipoUsuario = leer["TipoUsuario"].ToString(),
                                        Usuario = leer["Usuario"].ToString(),
                                        Contraseña = leer["Contraseña"].ToString()
                                    }
                                    );

                            }
                        }

                    }

                }
            }
            catch {

                Lista=new List<Usuarios>();
                
            }
            return Lista;

        }
 
        public DataTable MostrarUs()
        {
            DataTable dt = new DataTable();
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("ObtenerDatosUsuarios", ocn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                ocn.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

            }
            return dt;
        }

        public void InsertarU(string Nombre, string Apellido, DateTime FechaN, string TipoUsuario, string Usuario, string Contraseña )
        {
            using (SqlConnection ocn=new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("InsertarUsuario", ocn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre",Nombre);
                cmd.Parameters.AddWithValue("@Apellido",Apellido);
                cmd.Parameters.AddWithValue("@FechaNacimiento",FechaN);
                cmd.Parameters.AddWithValue("@TipoUsuario",TipoUsuario);
                cmd.Parameters.AddWithValue("@Usuario",Usuario);
                cmd.Parameters.AddWithValue("@Contraseña", Contraseña);
                cmd.ExecuteNonQuery(); 
                
            }

        }
        public void Eliminar(int ID)
        {
            using (SqlConnection ocn=new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("EliminarUsuario",ocn);
                cmd.CommandType=CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario",ID);
                cmd.ExecuteNonQuery();
            }
        }

        public void Modificar(int idUsuario,string Nombre, string Apellido, DateTime FechaN, string TipoUsuario, string Usuario, string Contraseña)
        {
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("ModificarUsuario", ocn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Apellido", Apellido);
                cmd.Parameters.AddWithValue("@FechaNacimiento", FechaN);
                cmd.Parameters.AddWithValue("@TipoUsuario", TipoUsuario);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                cmd.Parameters.AddWithValue("@Contraseña", Contraseña);
                cmd.ExecuteNonQuery();
            }
        }


    }

    
}
