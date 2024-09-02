using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDEdificios
    {
        public DataTable Mostrar() {
            DataTable dt = new DataTable();
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("ListarEdificios", ocn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                ocn.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

            }
            return dt;
        }

        public void InsertarE(string NombreEdificio)
        {
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("InsertarEdificio", ocn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreEdificio", NombreEdificio);
                cmd.ExecuteNonQuery();

            }

        }

        public void Modificar(int idEdificio, string NombreEdificio)
        {
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("EditarEdificio", ocn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEdificio", idEdificio);
                cmd.Parameters.AddWithValue("@NuevoNombreEdificio", NombreEdificio);
                cmd.ExecuteNonQuery();
            }

        }

        public void Eliminar(int idEdificio)
        {
            using (SqlConnection ocn = new SqlConnection(Conexion.cn))
            {
                ocn.Open();
                SqlCommand cmd = new SqlCommand("EliminarEdificio", ocn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idEdificio", idEdificio);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
