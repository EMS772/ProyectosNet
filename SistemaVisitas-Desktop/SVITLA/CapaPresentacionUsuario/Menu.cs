using CapaEntidad;
using CapaNegocio;
using System;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CapaPresentacionUsuario
{
    public partial class Menu : Form
    {
        private static string usuarioa;
        private static string tpusuario;
        public Menu(Usuarios objusuario)
        {
            usuarioa = objusuario.Nombre;
            tpusuario = objusuario.TipoUsuario;

            InitializeComponent();
        }
        private bool modse = false;

        private void Opciones_btn_Click(object sender, EventArgs e)
        {
            modse = !modse;
            if (modse) {
                CerrarSbtn.Visible = true; 
            }
            else
            {
                CerrarSbtn.Visible = false;
            }
        }

        private void CerrarSbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'bDVISITASDataSet1.Edificios' Puede moverla o quitarla según sea necesario.
            this.edificiosTableAdapter.Fill(this.bDVISITASDataSet1.Edificios);

            // TODO: esta línea de código carga datos en la tabla 'bDVISITASDataSet.Aulas' Puede moverla o quitarla según sea necesario.
            this.aulasTableAdapter.Fill(this.bDVISITASDataSet.Aulas);
            Nomlbl.Text = usuarioa.ToString();
            Tp_lbl.Text = $"({tpusuario})";

            CargarV();

        }

        private void Ingresar_btn_Click(object sender, EventArgs e)
        {
            CNVisitas OBJ = new CNVisitas();

            try
            {
              OBJ.insertar(aulacombo.Text, ecombo.Text, nom_txtbox.Text, apellido_txtbox.Text, catxtbox.Text, correo_txtbox.Text, HoraE.Value, HoraS.Value, Motxtbox.Text);
                Limpiar();
                CargarV();
            }
            catch (Exception ex){

                MessageBox.Show($"error por{ex}");
            }
        }

        private void Limpiar()
        {

            nom_txtbox.Clear();
            apellido_txtbox.Clear();
            catxtbox.Clear();
            correo_txtbox.Clear();
            Motxtbox.Clear();
        }
        private void CargarV()
        {
            CNVisitas objvisitas = new CNVisitas();
            dataGridView1.DataSource= objvisitas.Cargar();
        }
    }
}
