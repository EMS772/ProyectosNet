using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDVisitas
    {   
        /*#####################################################################################
                        Funcion Insertar visitantes
        ######################################################################################*/
        public void RegistrarV(string nombreAula, string nombreEdificio, string nombreVisitante, string apellidoVisitante, string carrera, string correo, DateTime horaEntrada, DateTime horaSalida, string motivoVisita)
        {
                using (SqlConnection ocn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarVisita", ocn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    ocn.Open();

                    cmd.Parameters.AddWithValue("@nombreAula",nombreAula);
                    cmd.Parameters.AddWithValue("@nombreEdificio",nombreEdificio);
                    cmd.Parameters.AddWithValue("@nombreVisitante",nombreVisitante);
                    cmd.Parameters.AddWithValue("@apellidoVisitante",apellidoVisitante);
                    cmd.Parameters.AddWithValue("@carrera",carrera);
                    cmd.Parameters.AddWithValue("@correo",correo);
                    cmd.Parameters.AddWithValue("@horaEntrada",horaEntrada);
                    cmd.Parameters.AddWithValue("@horaSalida",horaSalida);
                    cmd.Parameters.AddWithValue("@motivoVisita",motivoVisita);
                    cmd.ExecuteNonQuery();
                }

        }

        /*#####################################################################################
                                   Funcion Datatable
         ######################################################################################*/
        public DataTable Visitas()
        {
            DataTable dt = new DataTable(); 
            using (SqlConnection ocn= new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("ObtenerDatosVisitas",ocn);
                cmd.CommandType= System.Data.CommandType.StoredProcedure; 
                ocn.Open();

                using (SqlDataAdapter da=new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                dt.Columns["NombreAula"].ColumnName = "Aula";
                dt.Columns["NombreEdificio"].ColumnName = "Edificio";
                dt.Columns["NombreVisitante"].ColumnName = "Visitante";
            }
            return dt;
        }

    }
}
