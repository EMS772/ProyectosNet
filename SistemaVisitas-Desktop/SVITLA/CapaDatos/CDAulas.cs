using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDAulas
    {

        public DataTable ListarAulas()
        {
            DataTable dt = new DataTable();

            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("ListarAulas", ocn);
                cmd.CommandType = CommandType.StoredProcedure;

                ocn.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public void EliminarAulas(int ID)
        {
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("EliminarAula", ocn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", ID);

                cmd.ExecuteNonQuery();
            }
        }

        public void InsertarAulas(string NombreAula)
        {
            using (SqlConnection ocn= new SqlConnection(Conexion.cn)) {

                ocn.Open();
                SqlCommand cmd = new SqlCommand("InsertarAula", ocn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreAula",NombreAula);
                cmd.ExecuteNonQuery();

            }
        }

        public void Modificar(int IdAula,string NombreAula)
        {
            using (SqlConnection ocn=new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("EditarNombreAula", ocn);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAula",IdAula);
                cmd.Parameters.AddWithValue("@nuevoNombreAula",NombreAula);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
