using System;
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
                // Intentar leer la cadena completa desde variable de entorno "MONGO_CONN" y nombre de BD desde "MONGO_DB"
                var envConn = Environment.GetEnvironmentVariable("MONGO_CONN");
                var envDb = Environment.GetEnvironmentVariable("MONGO_DB");

                string connectionString;
                string databaseName;

                if (!string.IsNullOrWhiteSpace(envConn) && !string.IsNullOrWhiteSpace(envDb))
                {
                    connectionString = envConn;
                    databaseName = envDb;
                }
                else
                {
                    // Cadena provista por Atlas con placeholder
                    var atlasTemplate = "mongodb+srv://patitas:<db>@cluster0.uxnei2v.mongodb.net/?appName=Cluster0";

                    // Pedir contraseña al usuario (no se guarda)
                    var password = PromptInput("Ingresa la contraseña del usuario 'patitas' (no se guardará):", "Contraseña MongoDB", "");
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        MessageBox.Show("No se ingresó contraseña. Conexión cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Pedir nombre de la base de datos
                    databaseName = PromptInput("Ingresa el nombre de la base de datos a usar:", "Nombre BD", "PatitasDB");
                    if (string.IsNullOrWhiteSpace(databaseName))
                    {
                        MessageBox.Show("No se ingresó nombre de la base de datos. Conexión cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Reemplazar placeholder por contraseña (escape para seguridad)
                    var escaped = Uri.EscapeDataString(password);
                    connectionString = atlasTemplate.Replace("<db>", escaped);
                }

                // Conectar
                ConexionMongo.Connect(connectionString, databaseName);

                // Probar conexión
                if (ConexionMongo.TestConnection())
                {
                    var db = ConexionMongo.ObtenerConexion();
                    //MessageBox.Show("Conectado a MongoDB: " + db.DatabaseNamespace.DatabaseName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        // Helper simple para pedir entrada de texto sin depender de Microsoft.VisualBasic
        private static string PromptInput(string text, string caption, string defaultValue = "")
        {
            using (Form prompt = new Form())
            {
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterScreen;
                prompt.Width =420;
                prompt.Height =160;
                prompt.Text = caption;
                prompt.MinimizeBox = false;
                prompt.MaximizeBox = false;

                Label textLabel = new Label() { Left =10, Top =10, Width =380, Text = text };
                TextBox textBox = new TextBox() { Left =10, Top =35, Width =380, Text = defaultValue };
                Button confirmation = new Button() { Text = "OK", Left =220, Width =80, Top =70, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancelar", Left =310, Width =80, Top =70, DialogResult = DialogResult.Cancel };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);

                prompt.AcceptButton = confirmation;
                prompt.CancelButton = cancel;

                var result = prompt.ShowDialog();
                return result == DialogResult.OK ? textBox.Text : string.Empty;
            }
        }

        private void lblRecuperar_Click(object sender, EventArgs e)
        {
            Panel_Ingresar.Visible = false;
            panel_recuperar.Visible = true;
        }

        private void label6_Click(object sender, EventArgs e)
        {
           
            
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
            string id = txtIngresarusuario.Text.Trim(); 
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
            MessageBox.Show($"Bienvenido, {usuario.nombreUsuario} {usuario.apellidoUsuario}");

            if (usuario.rolUsuario == "Administrador")
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

