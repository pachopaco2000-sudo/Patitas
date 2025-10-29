using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Veterinaria
{
    public partial class Registro_Usuarios : Form
    {
        private IMongoCollection<Usuarios> coleccionUsuarios;

        public Registro_Usuarios()
        {
            InitializeComponent();
            ConectarMongo();
            CargarTiposDocumento();
            CargarTiposUsuario();
            ConfigurarValidaciones();
        }

        private void ConfigurarValidaciones()
        {
            // Solo letras y espacios para nombre
            txtNombre.KeyPress += SoloLetrasYEspacios;

            // Solo letras y espacios para apellidos
            txtApellidos.KeyPress += SoloLetrasYEspacios;

            // Solo números para teléfono
            txtTelefono.KeyPress += SoloNumeros;

            // Solo números para número de documento
            txtNumeroDocumento.KeyPress += SoloNumeros;

            // Validación especial para correo electrónico
            txtCorreo.Leave += ValidarCorreo;

            // Validación para contraseña
            txtContraseña.KeyPress += ValidarContraseña;
        }

        // Validación: Solo letras y espacios
        private void SoloLetrasYEspacios(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta letras y espacios.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Validación: Solo números
        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta números.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Validación: Correo electrónico
        private void ValidarCorreo(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            if (!string.IsNullOrEmpty(correo))
            {
                string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(correo, patronCorreo))
                {
                    MessageBox.Show("Por favor, ingresa un correo electrónico válido.", "Validación",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCorreo.Focus();
                    txtCorreo.SelectAll();
                }
            }
        }

        // Validación: Contraseña (puede contener letras, números y algunos caracteres especiales)
        private void ValidarContraseña(object sender, KeyPressEventArgs e)
        {
            // Permitir letras, números y algunos caracteres especiales comunes
            if (!char.IsLetterOrDigit(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                !"@#$%&*-_.".Contains(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras, números y los caracteres: @ # $ % & * - _ .",
                              "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarTiposDocumento()
        {
            cmbTipoDocumento.Items.AddRange(new string[] {
                "Cédula de Ciudadanía",
                "Cédula de Extranjería",
                "Tarjeta de Identidad",
                "Pasaporte"
            });
            cmbTipoDocumento.SelectedIndex = 0;
        }

        private void CargarTiposUsuario()
        {
            cmbTipoUsuario.Items.AddRange(new string[] {
                "Paciente",
                "Médico",
                "Administrativo"
            });
            cmbTipoUsuario.SelectedIndex = 0;
        }

        private void ConectarMongo()
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var baseDatos = cliente.GetDatabase("BD_MediCitas");
            coleccionUsuarios = baseDatos.GetCollection<Usuarios>("Usuarios");
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposCompletos())
                return;

            string nombres = txtNombre.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string tipoDocumento = cmbTipoDocumento.SelectedItem?.ToString();
            string numeroDocumento = txtNumeroDocumento.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();
            string tipoUsuario = cmbTipoUsuario.SelectedItem?.ToString();

            // Validaciones adicionales
            if (!ValidarLongitudContraseña(contraseña))
                return;

            if (!ValidarCorreoElectronico(correo))
                return;

            if (!ValidarDocumentoUnico(numeroDocumento))
                return;

            if (!ValidarCorreoUnico(correo))
                return;

            // Crear y guardar usuario
            if (CrearUsuario(nombres, apellidos, telefono, tipoDocumento, numeroDocumento, correo, contraseña, tipoUsuario))
            {
                MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                Regresar();
            }
        }

        private bool ValidarCamposCompletos()
        {
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()) ||
                string.IsNullOrEmpty(txtApellidos.Text.Trim()) ||
                string.IsNullOrEmpty(txtTelefono.Text.Trim()) ||
                string.IsNullOrEmpty(txtNumeroDocumento.Text.Trim()) ||
                string.IsNullOrEmpty(txtCorreo.Text.Trim()) ||
                string.IsNullOrEmpty(txtContraseña.Text.Trim()))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarLongitudContraseña(string contraseña)
        {
            if (contraseña.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarCorreoElectronico(string correo)
        {
            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(correo, patronCorreo))
            {
                MessageBox.Show("Por favor, ingresa un correo electrónico válido.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarDocumentoUnico(string numeroDocumento)
        {
            var existenteDoc = coleccionUsuarios.Find(u => u.numeroDocumento == numeroDocumento).FirstOrDefault();
            if (existenteDoc != null)
            {
                MessageBox.Show("Este número de documento ya está registrado.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarCorreoUnico(string correo)
        {
            var existente = coleccionUsuarios.Find(u => u.correo == correo).FirstOrDefault();
            if (existente != null)
            {
                MessageBox.Show("Este correo ya está registrado.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool CrearUsuario(string nombres, string apellidos, string telefono, string tipoDocumento,
                                string numeroDocumento, string correo, string contraseña, string tipoUsuario)
        {
            try
            {
                // Generar nuevo ID
                int nuevoNumero = 1;
                var ultimo = coleccionUsuarios.AsQueryable()
                    .OrderByDescending(u => u._id)
                    .FirstOrDefault();

                if (ultimo != null)
                {
                    string numeroTexto = new string(ultimo._id.SkipWhile(c => !char.IsDigit(c)).ToArray());
                    int.TryParse(numeroTexto, out nuevoNumero);
                    nuevoNumero++;
                }

                string nuevoId = $"U{nuevoNumero:D3}";

                // Crear el nuevo usuario
                Usuarios nuevoUsuario = new Usuarios
                {
                    _id = nuevoId,
                    nombre = nombres,
                    apellidos = apellidos,
                    tipoDocumento = tipoDocumento,
                    numeroDocumento = numeroDocumento,
                    telefono = telefono,
                    correo = correo,
                    contraseña = contraseña,
                    rol = tipoUsuario
                };

                // Insertar en la colección
                coleccionUsuarios.InsertOne(nuevoUsuario);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el usuario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtNumeroDocumento.Clear();
            txtCorreo.Clear();
            txtContraseña.Clear();
            cmbTipoDocumento.SelectedIndex = 0;
            cmbTipoUsuario.SelectedIndex = 0;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Regresar();
        }

        public void Regresar()
        {
            Form1 F = new Form1();
            F.Show();
            this.Hide();
        }

        // Evento para limitar la longitud del teléfono
        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            if (txtTelefono.Text.Length > 10)
            {
                txtTelefono.Text = txtTelefono.Text.Substring(0, 10);
                txtTelefono.SelectionStart = 10;
                MessageBox.Show("El teléfono no puede tener más de 10 dígitos.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Evento para limitar la longitud del documento
        private void txtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            if (txtNumeroDocumento.Text.Length > 15)
            {
                txtNumeroDocumento.Text = txtNumeroDocumento.Text.Substring(0, 15);
                txtNumeroDocumento.SelectionStart = 15;
                MessageBox.Show("El número de documento no puede tener más de 15 dígitos.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}