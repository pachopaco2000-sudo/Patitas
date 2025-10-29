using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Veterinaria
{
    public partial class Form1 : Form
    {
  

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 

            try
            {
                var db = ConexionMongo.ObtenerConexion();
                //MessageBox.Show("Conectado a MongoDB: " + db.DatabaseNamespace.DatabaseName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        private void lblRecuperar_Click(object sender, EventArgs e)
        {
            Panel_Ingresar.Visible = false;
            panel_recuperar.Visible = true;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Registro_Usuarios RU = new Registro_Usuarios();
            RU.Show();
            this.Hide();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quiere salir?", " Salir ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

   

        private void lblRegistroPRecuperar_Click(object sender, EventArgs e)
        {
            Panel_Registro.Visible = true;
            panel_recuperar.Visible = false;
        }

        private void lblIngresarPRecuperar_Click(object sender, EventArgs e)
        {
            Panel_Ingresar.Visible = true;
            panel_recuperar.Visible = false;
        }

        private void lblRecuperar2_Click(object sender, EventArgs e)
        {
            panel_recuperar.Visible = true;
            Panel_Registro.Visible = false;
        }

        private void lblRegistro2_Click(object sender, EventArgs e)
        {

        }

        private void lblRecuperar_Click_1(object sender, EventArgs e)
        {
            panel_recuperar.Visible = true;
            Panel_Ingresar.Visible = false;
        }

        private void lblIngresar2_Click(object sender, EventArgs e)
        {
            Panel_Ingresar.Visible = true;
            Panel_Registro.Visible = false;
        }

        private void lblRegistro_Click(object sender, EventArgs e)
        {
            Panel_Registro.Visible = true;
            Panel_Ingresar.Visible = false;
        }

        private void lblRecuperarPRecuperar_Click(object sender, EventArgs e)
        {

        }

        private void btnIniciarsesion_Click_1(object sender, EventArgs e)
        {
            string id = txtIngresarusuario.Text.Trim(); // Ahora será número de documento
            string contraseña = txtcontraseñaingresar.Text.Trim();

            // Validaciones básicas
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Por favor, ingresa tu número de documento y contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Creamos instancia de la clase que consulta MongoDB
            QueryUsuario query = new QueryUsuario();

            // Verificamos si el usuario existe por número de documento
            var usuario = query.ObtenerUsuario(id);

            if (usuario == null)
            {
                MessageBox.Show("Número de documento no registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (usuario.contraseña != contraseña)
            {
                MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Si todo es correcto, redirigimos según el rol
            MessageBox.Show($"Bienvenido, {usuario.nombre} {usuario.apellidos}");

            if (usuario.rol == "Administrador")
            {
                FrmAdmin frmAdmin = new FrmAdmin();
                frmAdmin.Show();
                this.Hide();
            }
            else // cualquier otro rol abre citas
            {
                try
                {
                    FrmCitas pp = new FrmCitas(usuario);
                    pp.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir citas: {ex.Message}\n\nProbable problema en DataGridView.",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}

