using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacionAdmin
{
    public partial class Edificios : Form
    {
        private bool modse = false;
        static string IDEdificio = null;

        public Edificios()
        {
            InitializeComponent();
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
                Limpiar();
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
                Limpiar();
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
                panelNombre.Enabled = false;
                In_btn.Enabled = false;
                Mod_btn.Enabled = false;
                Eliminar2_btn.Enabled = true;
                Eliminar2_btn.BackColor = Color.DarkRed;

            }
            else
            {
                Limpiar();
                panelNombre.Enabled = true;
                In_btn.Enabled = true;
                Mod_btn.Enabled = true;
                Eliminar2_btn.Enabled = false;
                Eliminar2_btn.BackColor = Color.FromArgb(4, 59, 123);
            }
        }

        private void Opciones_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Mostrar()
        {
            CNEdificios objedificios = new CNEdificios();

            dataGridViewA.DataSource = objedificios.MostrarE();
        }

        private void Edificios_Load(object sender, EventArgs e)
        {
            Mostrar();
        }

        private void Ingresar2_btn_Click(object sender, EventArgs e)
        {
            CNEdificios objedificios=new CNEdificios();
            try
            {
                objedificios.InsertarE(Edificio_txt.Text);
                Mostrar();
                Limpiar();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"error por {ex}");
            }

        }

        private void Modificar2_btn_Click(object sender, EventArgs e)
        {
            CNEdificios objedificios=new CNEdificios();
            objedificios.ModificarA(IDEdificiotxt.Text ,Edificio_txt.Text);
            Mostrar();
            Limpiar();
        }

        private void TraspasarD_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewA.SelectedRows.Count > 0)
                {

                    Edificio_txt.Text = dataGridViewA.CurrentRow.Cells["NombreEdificio"].Value.ToString();
                    IDEdificiotxt.Text = dataGridViewA.CurrentRow.Cells["idEdificio"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"error por {ex}");

            }
        }

        private void Eliminar2_btn_Click(object sender, EventArgs e)
        {
            CNEdificios Aulas = new CNEdificios();
            IDEdificio = dataGridViewA.CurrentRow.Cells["idEdificio"].Value.ToString();
            try
            {
                if (dataGridViewA.SelectedRows.Count > 0)
                {
                    Aulas.Eliminar(IDEdificio);
                    Mostrar();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error por:{ex}");
            }
        }

        private void Limpiar()
        {
            IDEdificiotxt.Clear();
            Edificio_txt.Clear();
        }
    }
}
