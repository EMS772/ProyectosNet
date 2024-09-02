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
using CapaPresentacionAdmin;

namespace CapaPresentacionUsuario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Ingresar_btn_Click(object sender, EventArgs e)
        {
            List<Usuarios> Login = new CNUsuario().Listar();

            Usuarios ousuario = new CNUsuario().Listar().Where(u => u.Usuario == Usuario_txt.Text && u.Contraseña==Contra_txt.Text ).FirstOrDefault() ;

            try
            {
                if (ousuario != null)
                {
                    if (ousuario.TipoUsuario == "General")
                    {
                        Menu menu = new Menu(ousuario);
                        menu.Show();
                        this.Hide();

                        menu.FormClosing += Frmclosing;

                    }
                    else if (ousuario.TipoUsuario == "Administrador")
                    {
                        Admin formAdmin = new Admin(ousuario);
                        formAdmin.Show();
                        this.Hide();
                        formAdmin.FormClosing += Frmclosing;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error por{ex}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


        }

        private void Frmclosing(object sender, FormClosingEventArgs e)
        {
            Usuario_txt.Text = "";
            Contra_txt.Text = "";

            this.Show();
        }

       
    }
}
