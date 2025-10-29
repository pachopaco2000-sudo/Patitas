using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;

namespace Veterinaria
{
    public partial class FrmAdmin : Form
    {
        // CONEXIÓN A LA BASE DE DATOS
        private readonly IMongoCollection<Usuarios> _usuariosCollection; // Conexión a la colección de usuarios en MongoDB
        private List<Usuarios> _usuariosActuales; // Lista para almacenar los usuarios cargados
        private string _usuarioSeleccionadoId;
        private readonly IMongoCollection<Cita> _citasCollection; // SOLO AGREGUE ESTA LÍNEA


        public FrmAdmin()
        {
            InitializeComponent();
            // ESTABLECER CONEXIÓN CON MONGODB
            //var database = ConexionMongo.ObtenerConexion();
            //_usuariosCollection = database.GetCollection<Usuarios>("Usuarios");
            //_citasCollection = database.GetCollection<Cita>("Citas"); // SOLO AGREGUE ESTA LÍNEA
            //ConfigurarDataGrid(); // Configurar apariencia del DataGridView
        }
        // CONFIGURAR LA APARIENCIA Y COMPORTAMIENTO DEL DATAGRIDVIEW
        private void ConfigurarDataGrid()
        {
            // CONFIGURACIÓN BÁSICA
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.ReadOnly = false;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.RowHeadersVisible = false;

            // HABILITAR SCROLL VERTICAL Y HORIZONTAL
            dgvUsuarios.ScrollBars = ScrollBars.Both;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Cambiado para mejor control

            // ESTILOS VISUALES
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(59, 130, 246);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 8.5f);

            // MEJORAR RENDERIZADO
            dgvUsuarios.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // AL CARGAR EL FORMULARIO, CARGAR TODOS LOS USUARIOS
        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            CargarTodosLosUsuarios(); // Cargar datos iniciales
            ActualizarContadoresCitas(); // SOLO AGREGUE ESTA LÍNEA
        }
        private void CargarTodosLosUsuarios()
        {
            try
            {
                // EXCLUIR AL USUARIO ADMINISTRATIVO ACTUAL DEL LISTADO
                _usuariosActuales = _usuariosCollection.Find(u => u.rol != "Administrativo").ToList();
                dgvUsuarios.DataSource = _usuariosActuales;
                ConfigurarColumnasDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CONFIGURAR LAS COLUMNAS DEL DATAGRIDVIEW (ENCABEZADOS, ANCHOS, ETC.)
        private void ConfigurarColumnasDataGrid()
        {
            if (dgvUsuarios.Columns.Count > 0)
            {
                // CONFIGURAR ANCHOS DE COLUMNAS PARA MEJOR VISUALIZACIÓN
                dgvUsuarios.Columns["_id"].HeaderText = "ID";
                dgvUsuarios.Columns["_id"].ReadOnly = true;
                dgvUsuarios.Columns["_id"].Width = 50;
                dgvUsuarios.Columns["_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvUsuarios.Columns["nombre"].HeaderText = "Nombres";
                dgvUsuarios.Columns["nombre"].Width = 120;
                dgvUsuarios.Columns["nombre"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvUsuarios.Columns["apellidos"].HeaderText = "Apellidos";
                dgvUsuarios.Columns["apellidos"].Width = 120;
                dgvUsuarios.Columns["apellidos"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvUsuarios.Columns["tipoDocumento"].HeaderText = "Tipo Doc";
                dgvUsuarios.Columns["tipoDocumento"].Width = 100;

                dgvUsuarios.Columns["numeroDocumento"].HeaderText = "Cédula";
                dgvUsuarios.Columns["numeroDocumento"].Width = 90;
                dgvUsuarios.Columns["numeroDocumento"].ReadOnly = true;

                dgvUsuarios.Columns["telefono"].HeaderText = "Teléfono";
                dgvUsuarios.Columns["telefono"].Width = 90;

                dgvUsuarios.Columns["correo"].HeaderText = "Correo";
                dgvUsuarios.Columns["correo"].Width = 180;
                dgvUsuarios.Columns["correo"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //dgvUsuarios.Columns["contraseña"].HeaderText = "Contraseña";
                //dgvUsuarios.Columns["contraseña"].Width = 100;
                dgvUsuarios.Columns["contraseña"].Visible = false;
                dgvUsuarios.Columns["rol"].HeaderText = "Rol";
                dgvUsuarios.Columns["rol"].Width = 80;
            }
        }

        // BOTÓN BUSCAR - BUSCAR USUARIO POR NÚMERO DE CÉDULA
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string cedula = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(cedula))
            {
                MessageBox.Show("Por favor, ingrese un número de cédula para buscar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var usuario = _usuariosCollection.Find(u => u.numeroDocumento == cedula).FirstOrDefault();

                if (usuario != null)
                {
                    dgvUsuarios.DataSource = new List<Usuarios> { usuario };

                    // ✅ AQUÍ LLAMAS AL MÉTODO CON EL ID DEL USUARIO ENCONTRADO
                    ActualizarContadoresUsuarioEspecifico(usuario._id);
                }
                else
                {
                    MessageBox.Show("No se encontró ningún usuario con esa cédula.", "Búsqueda sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodosLosUsuarios();
                    ActualizarContadoresCitas(); // Volver a contadores generales
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DOBLE CLICK EN UNA CELDA PARA INICIAR EDICIÓN DIRECTA

        private void btnBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dgvUsuarios_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvUsuarios.BeginEdit(true);
            }
        }

        // LIMPIAR BÚSQUEDA
        private void txtBuscar_DoubleClick(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarTodosLosUsuarios();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            CargarTodosLosUsuarios();
            txtBuscar.Clear();

            //AQUÍ VUELVES A LOS CONTADORES GENERALES
            ActualizarContadoresCitas();

            MessageBox.Show("Mostrando todos los usuarios.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // ACTUALIZAR CONTADORES DE CITAS PARA TODOS LOS USUARIOS
        private void ActualizarContadoresCitas()
        {
            try
            {
                // OBTENER TODAS LAS CITAS
                var todasLasCitas = _citasCollection.Find(_ => true).ToList();

                // CONTAR CITAS TOTALES
                int totalCitas = todasLasCitas.Count;

                // CONTAR CITAS PENDIENTES (fecha futura y estado Pendiente)
                int citasPendientes = todasLasCitas.Count(c =>
                    DateTime.Parse(c.Fecha) >= DateTime.Today &&
                    c.Estado == "Pendiente");

                // CONTAR CITAS CONFIRMADAS (fecha futura y estado Confirmada o Reprogramada)
                int citasConfirmadas = todasLasCitas.Count(c =>
                    DateTime.Parse(c.Fecha) >= DateTime.Today &&
                    (c.Estado == "Confirmada" || c.Estado == "Reprogramada"));

                // CONTAR CITAS COMPLETADAS (fecha pasada o estado Completada)
                int citasCompletadas = todasLasCitas.Count(c =>
                    DateTime.Parse(c.Fecha) < DateTime.Today ||
                    c.Estado == "Completada");

                // ACTUALIZAR LOS LABELS
                lblTotalCitas.Text = totalCitas.ToString();
                lblCitasPendientes.Text = citasPendientes.ToString();
                lblCitasConfirmadas.Text = citasConfirmadas.ToString();
                lblCitasCompletadas.Text = citasCompletadas.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar contadores: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);

                // VALORES POR DEFECTO EN CASO DE ERROR
                lblTotalCitas.Text = "0";
                lblCitasPendientes.Text = "0";
                lblCitasConfirmadas.Text = "0";
                lblCitasCompletadas.Text = "0";
            }
        }

        // ACTUALIZAR CONTADORES PARA UN USUARIO ESPECÍFICO (cuando se busca)
        private void ActualizarContadoresUsuarioEspecifico(string usuarioId)
        {
            try
            {
                // OBTENER CITAS DEL USUARIO ESPECÍFICO
                var citasUsuario = _citasCollection.Find(c => c.UsuarioId == usuarioId).ToList();

                // CONTAR CITAS DEL USUARIO
                int totalCitas = citasUsuario.Count;

                int citasPendientes = citasUsuario.Count(c =>
                    DateTime.Parse(c.Fecha) >= DateTime.Today &&
                    c.Estado == "Pendiente");

                int citasConfirmadas = citasUsuario.Count(c =>
                    DateTime.Parse(c.Fecha) >= DateTime.Today &&
                    (c.Estado == "Confirmada" || c.Estado == "Reprogramada"));

                int citasCompletadas = citasUsuario.Count(c =>
                    DateTime.Parse(c.Fecha) < DateTime.Today ||
                    c.Estado == "Completada");

                // ACTUALIZAR LOS LABELS
                lblTotalCitas.Text = totalCitas.ToString();
                lblCitasPendientes.Text = citasPendientes.ToString();
                lblCitasConfirmadas.Text = citasConfirmadas.ToString();
                lblCitasCompletadas.Text = citasCompletadas.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar contadores del usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as Usuarios;
                if (usuarioSeleccionado != null)
                {
                    //ACTUALIZAR CONTADORES DEL USUARIO SELECCIONADO
                    ActualizarContadoresUsuarioEspecifico(usuarioSeleccionado._id);
                }
            }
        }
    }
}