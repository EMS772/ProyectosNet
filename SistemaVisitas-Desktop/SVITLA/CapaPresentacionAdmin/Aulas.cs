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
    public partial class Aulas : Form
    {
        public Aulas()
        {
            InitializeComponent();
        }

        bool modse=false;
        static string IDaula = null;
        private void Opciones_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Aulas_Load(object sender, EventArgs e)
        {
                
          MostrarAulas();
            
        }

        private void MostrarAulas()
        {
            try
            {
                CNAulas ObjAulas = new CNAulas();
                dataGridViewA.DataSource = ObjAulas.Aulas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void In_btn_Click(object sender, EventArgs e)
        {
            modse=!modse;

            if (modse)
            {
                Mod_btn.Enabled = false;
                Eliminar_btn.Enabled=false;
                Ingresar2_btn.BackColor = Color.DarkGreen;
                Ingresar2_btn.Enabled = true;

            }
            else
            {
                Limpiar();
                Mod_btn.Enabled = true;
                Eliminar_btn.Enabled = true;
                Ingresar2_btn.Enabled = false;
                Ingresar2_btn.BackColor= Color.FromArgb(4, 59, 123);
            }

        }

        private void Modificar2_btn_Click(object sender, EventArgs e)
        {
            CNAulas objAulas = new CNAulas();
            objAulas.ModificarA(Aulas_txtbox.Text,AulaN_txt.Text);
            MostrarAulas();
            Limpiar();

        }

        private void Mod_btn_Click(object sender, EventArgs e)
        {
            modse = !modse;
            if (modse) {
                In_btn.Enabled = false;
                Eliminar_btn.Enabled = false;
                Modificar2_btn.Enabled = true;
                Modificar2_btn.BackColor=Color.Goldenrod;
                TraspasarD_btn.Enabled = true;
                TraspasarD_btn.BackColor= Color.Goldenrod;
            }
            else
            {
                Limpiar();
                In_btn.Enabled = true;
                Eliminar_btn.Enabled = true;
                Modificar2_btn.Enabled= false;
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

        private void Eliminar2_btn_Click(object sender, EventArgs e)
        {
            CNAulas Aulas = new CNAulas();
            IDaula = dataGridViewA.CurrentRow.Cells["idAula"].Value.ToString();
            try
            {
                if (dataGridViewA.SelectedRows.Count > 0)
                {
                    Aulas.Eliminar(IDaula);
                    MostrarAulas();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error por:{ex}");
            }
        }

        private void Ingresar2_btn_Click(object sender, EventArgs e)
        {
            CNAulas objAulas = new CNAulas();
            try
            {
                objAulas.InsertarA(AulaN_txt.Text);
                MostrarAulas();
                Limpiar();
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
                if (dataGridViewA.SelectedRows.Count > 0)
                {

                    AulaN_txt.Text = dataGridViewA.CurrentRow.Cells["NombreAula"].Value.ToString();
                    Aulas_txtbox.Text = dataGridViewA.CurrentRow.Cells["idAula"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"error por {ex}");

            }
        }
        private void Limpiar()
        {
            AulaN_txt.Clear();
            Aulas_txtbox.Clear();
        }
    }
}
