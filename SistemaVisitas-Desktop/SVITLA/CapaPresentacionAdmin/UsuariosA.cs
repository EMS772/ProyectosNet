using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CapaPresentacionAdmin
{
    public partial class Admin : Form
    {
        private static string usuarioa;
        private static string tpusuario;
        private bool modse = false;
        private static string IDUsuario = null;
        public Admin(Usuarios objusuario)
        {
            usuarioa = objusuario.Nombre;
            tpusuario = objusuario.TipoUsuario;
            InitializeComponent();
        }

        private void Aulas_btn_Click(object sender, EventArgs e)
        {
            Aulas aulas = new Aulas();
            aulas.Show();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            MostrarU();
            Nomlbl.Text = usuarioa.ToString();
            Tp_lbl.Text = $"({tpusuario})";
        }

        private void MostrarU()
        {
            CNUsuario objusu = new CNUsuario();

            dataGridViewS.DataSource = objusu.MostrarU();
        }
        private void CerrarSbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Opciones_btn_Click(object sender, EventArgs e)
        {
            modse = !modse;
            if (modse)
            {
                CerrarSbtn.Visible = true;
            }
            else
            {
                CerrarSbtn.Visible = false;
            }
        }

        private void In_btn_Click(object sender, EventArgs e)
        {
            modse = !modse;

            if (modse)
            {
                Mod_btn.Enabled = false;
                Eliminar_btn.Enabled = false;
                Ingresar2_btn.BackColor = Color.DarkGreen;
                Ingresar2_btn.Enabled = true;

            }
            else
            {
                Limpiart();
                Mod_btn.Enabled = true;
                Eliminar_btn.Enabled = true;
                Ingresar2_btn.Enabled = false;
                Ingresar2_btn.BackColor = Color.FromArgb(4, 59, 123);
            }
        }

        private void Mod_btn_Click(object sender, EventArgs e)
        {
            modse = !modse;
            if (modse)
            {

                In_btn.Enabled = false;
                Eliminar_btn.Enabled = false;
                Modificar2_btn.Enabled = true;
                Modificar2_btn.BackColor = Color.Goldenrod;
                TraspasarD_btn.Enabled = true;
                TraspasarD_btn.BackColor = Color.Goldenrod;
            }
            else
            {
                Limpiart();
                In_btn.Enabled = true;
                Eliminar_btn.Enabled = true;
                Modificar2_btn.Enabled = false;
                Modificar2_btn.BackColor = Color.FromArgb(4, 59, 123);
                TraspasarD_btn.Enabled = false;
                TraspasarD_btn.BackColor = Color.FromArgb(4, 59, 123);

            }
        }

        private void Eliminar_btn_Click(object sender, EventArgs e)
        {
            modse = !modse;

            if (modse)
            {
                In_btn.Enabled = false;
                Mod_btn.Enabled = false;
                Eliminar2_btn.Enabled = true;
                Eliminar2_btn.BackColor = Color.DarkRed;
                nom_txtbox.Enabled = false;
                Nombrelbl.Enabled = false;
                apelbl.Enabled = false;
                apellido_txtbox.Enabled = false;
                tpulbl.Enabled = false;
                tipoutxt.Enabled = false;
                fechalbl.Enabled = false;
                Fecha_n.Enabled = false;
                correo_txtbox.Enabled = false;
                ulbl.Enabled = false;
                Contralbl.Enabled = false;
                Contratxt.Enabled = false;
                idulbl.Enabled = false;
                Idtxt.Enabled = false;

            }
            else
            {
                Limpiart();
                In_btn.Enabled = true;
                Mod_btn.Enabled = true;
                Eliminar2_btn.Enabled = false;
                Eliminar2_btn.BackColor = Color.FromArgb(4, 59, 123);
                nom_txtbox.Enabled = true;
                Nombrelbl.Enabled = true;
                apelbl.Enabled = true;
                apellido_txtbox.Enabled = true;
                tpulbl.Enabled = true;
                tipoutxt.Enabled = true;
                fechalbl.Enabled = true;
                Fecha_n.Enabled = true;
                correo_txtbox.Enabled = true;
                ulbl.Enabled = true;
                Contralbl.Enabled = true;
                Contratxt.Enabled = true;
                idulbl.Enabled = true;
                Idtxt.Enabled = true;
            }
        }

        private void Ingresar2_btn_Click(object sender, EventArgs e)
        {
            CNUsuario objusuario = new CNUsuario();
            objusuario.Insertaru(nom_txtbox.Text, apellido_txtbox.Text, Fecha_n.Value, tipoutxt.Text, correo_txtbox.Text, Contratxt.Text);
            MostrarU();
            Limpiart();
        }

        private void Eliminar2_btn_Click(object sender, EventArgs e)
        {
            CNUsuario objusuario = new CNUsuario();

            IDUsuario = dataGridViewS.CurrentRow.Cells["idUsuario"].Value.ToString();

            try
            {
                if (dataGridViewS.SelectedRows.Count > 0)
                {
                    objusuario.Eliminar(IDUsuario);
                    MostrarU();
                    Limpiart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error por:{ex}");
            }
        }

        private void TraspasarD_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewS.SelectedRows.Count > 0)
                {

                    Idtxt.Text = dataGridViewS.CurrentRow.Cells["idUsuario"].Value.ToString();
                    nom_txtbox.Text = dataGridViewS.CurrentRow.Cells["Nombre"].Value.ToString();
                    apellido_txtbox.Text = dataGridViewS.CurrentRow.Cells["Apellido"].Value.ToString();
                    Fecha_n.Text = dataGridViewS.CurrentRow.Cells["FechaNacimiento"].Value.ToString();
                    tipoutxt.Text = dataGridViewS.CurrentRow.Cells["TipoUsuario"].Value.ToString();
                    correo_txtbox.Text = dataGridViewS.CurrentRow.Cells["Usuario"].Value.ToString();
                    Contratxt.Text = dataGridViewS.CurrentRow.Cells["Contraseña"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"error por {ex}");

            }
        }
        private void Limpiart()
        {
            Idtxt.Clear();
            nom_txtbox.Clear();
            apellido_txtbox.Clear();
            tipoutxt.Clear();
            correo_txtbox.Clear();
            Contratxt.Clear();
        }

        private void Edificios_Click(object sender, EventArgs e)
        {
            Edificios edi= new Edificios();
            edi.Show();
            Limpiart();
        }

        private void Modificar2_btn_Click(object sender, EventArgs e)
        {
            CNUsuario objusuario = new CNUsuario();
            objusuario.Modificar(Idtxt.Text, nom_txtbox.Text, apellido_txtbox.Text, Fecha_n.Value, tipoutxt.Text, correo_txtbox.Text, Contratxt.Text);
            MostrarU();
            Limpiart();
        }
    }
}
