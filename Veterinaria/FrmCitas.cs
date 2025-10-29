﻿using MongoDB.Bson;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Veterinaria
{
    public partial class FrmCitas : Form
    {
        private readonly CitaService _citaService;

        // Horario de atención (puedes ajustarlo)
        private readonly TimeSpan _horaInicio = new TimeSpan(6, 0, 0); // 6:00 AM
        private readonly TimeSpan _horaFin = new TimeSpan(18, 0, 0);   // 6:00 PM
        private readonly TimeSpan _intervalo = new TimeSpan(0, 30, 0);  // 30 minuros

        private readonly Usuarios _usuarioActual; // Usuario logueado

        // AGREGAR variable para controlar cita seleccionada
        private string _citaSeleccionadaId = null;

        public FrmCitas(Usuarios usuario)
        {
            try
            {
                InitializeComponent();

                var database = ConexionMongo.ObtenerConexion();
                _citaService = new CitaService(database);
                _usuarioActual = usuario;

                ConfigurarDataGrid();
                CargarCitasUsuario(_usuarioActual._id.ToString());

                dgvCitas.SelectionChanged += dgvCitas_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ConfigurarDataGrid()
        {
            // 🔒 Bloquear edición y redimensión:
            dgvCitas.ReadOnly = true;                        // Solo lectura
            dgvCitas.AllowUserToAddRows = false;             // No permitir agregar filas manualmente
            dgvCitas.AllowUserToDeleteRows = false;          // No permitir eliminar desde el grid
            dgvCitas.AllowUserToResizeRows = false;          // No permitir cambiar el alto de filas
            dgvCitas.AllowUserToResizeColumns = false;       // No permitir cambiar ancho de columnas
            dgvCitas.AllowUserToOrderColumns = false;        // No permitir mover columnas
            dgvCitas.MultiSelect = false;                    // Solo una fila seleccionada
            dgvCitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección por fila completa
            dgvCitas.RowHeadersVisible = false;              // Oculta el borde izquierdo de numeración
            dgvCitas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajuste automático de columnas

            // === Configuración general ===
            dgvCitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCitas.MultiSelect = false;
            dgvCitas.ReadOnly = true;
            dgvCitas.AllowUserToAddRows = false;
            dgvCitas.AllowUserToDeleteRows = false;
            dgvCitas.RowHeadersVisible = false;
            dgvCitas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCitas.BorderStyle = BorderStyle.None;
            dgvCitas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCitas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCitas.BackgroundColor = Color.White;

            // === Encabezados de columna ===
            dgvCitas.EnableHeadersVisualStyles = false;
            dgvCitas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(58, 134, 255); // azul moderno
            dgvCitas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCitas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvCitas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCitas.ColumnHeadersHeight = 35;

            // === Filas ===
            dgvCitas.RowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250); // gris muy claro
            dgvCitas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 255); // tono azulado suave
            dgvCitas.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvCitas.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgvCitas.DefaultCellStyle.Padding = new Padding(5, 2, 5, 2);

            // === Efectos al seleccionar ===
            dgvCitas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(58, 134, 255); // mismo azul del encabezado
            dgvCitas.DefaultCellStyle.SelectionForeColor = Color.White;

            // === Ajustes de filas ===
            dgvCitas.RowTemplate.Height = 30;
            dgvCitas.GridColor = Color.FromArgb(220, 225, 230); // líneas sutiles
            dgvCitas.ScrollBars = ScrollBars.Both;

            // === Estilo al pasar el mouse ===
            dgvCitas.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dgvCitas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(225, 235, 255);
            };

            dgvCitas.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dgvCitas.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        e.RowIndex % 2 == 0 ? Color.FromArgb(245, 247, 250) : Color.FromArgb(235, 240, 255);
            };
        }


        private void ValidarFinDeSemana(object sender, EventArgs e)
        {
            DateTime fechaSeleccionada = dtpFechaHora.Value;

            if (fechaSeleccionada.DayOfWeek == DayOfWeek.Saturday)
            {
                dtpFechaHora.Value = fechaSeleccionada.AddDays(2); // Lunes
                MessageBox.Show("Las citas no están disponibles los sábados. Se ha seleccionado el lunes.",
                              "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (fechaSeleccionada.DayOfWeek == DayOfWeek.Sunday)
            {
                dtpFechaHora.Value = fechaSeleccionada.AddDays(1); // Lunes
                MessageBox.Show("Las citas no están disponibles los domingos. Se ha seleccionado el lunes.",
                              "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarEstadoBotones()
        {
            try
            {
                bool hayCitaSeleccionada = !string.IsNullOrEmpty(_citaSeleccionadaId);
                bool hayHorariosDisponibles = cmbHora.Items.Count > 0 && cmbHora.Enabled;

                btnEditar.Enabled = hayCitaSeleccionada;
                btnCancelar.Enabled = hayCitaSeleccionada;
                btnActualizar.Enabled = hayCitaSeleccionada && hayHorariosDisponibles;
                btnConfirmar.Enabled = !hayCitaSeleccionada && hayHorariosDisponibles;

                // Cambiar colores para mejor feedback visual
                btnConfirmar.BackColor = btnConfirmar.Enabled ? Color.MediumSeaGreen : Color.LightGray;
                btnCancelar.BackColor = btnCancelar.Enabled ? Color.LightCoral : Color.LightGray;
                btnEditar.BackColor = btnEditar.Enabled ? Color.Khaki : Color.LightGray;
                btnActualizar.BackColor = btnActualizar.Enabled ? Color.CornflowerBlue : Color.LightGray;
            }
            catch (Exception ex)
            {
                // En caso de error, deshabilitar todos los botones
                btnEditar.Enabled = false;
                btnCancelar.Enabled = false;
                btnActualizar.Enabled = false;
                btnConfirmar.Enabled = false;
            }
        }

        public void CargarCitasUsuario(string usuarioId)
        {
            try
            {
                var citas = _citaService.ObtenerCitasUsuario(usuarioId);

                // VERIFICAR SI LA LISTA ESTÁ VACÍA
                if (citas == null || !citas.Any())
                {
                    // Limpiar el DataGridView y mostrar mensaje
                    dgvCitas.DataSource = null;
                    dgvCitas.Rows.Clear();

                    // Agregar una fila indicando que no hay citas
                    dgvCitas.Columns.Clear();
                    dgvCitas.Columns.Add("Mensaje", "Información");
                    dgvCitas.Rows.Add("No tienes citas programadas");

                    dgvCitas.Columns["Mensaje"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Limpiar selección
                    dgvCitas.ClearSelection();
                    _citaSeleccionadaId = null;
                    ActualizarEstadoBotones();
                    return;
                }

                dgvCitas.DataSource = citas;

                // Personalizar encabezados SOLO si hay datos
                if (dgvCitas.Columns.Count > 0)
                {
                    dgvCitas.Columns["_id"].Visible = false;
                    dgvCitas.Columns["Motivo"].HeaderText = "Motivo";
                    dgvCitas.Columns["Fecha"].HeaderText = "Fecha";
                    dgvCitas.Columns["Hora"].HeaderText = "Hora";
                    dgvCitas.Columns["Estado"].HeaderText = "Estado";
                    dgvCitas.Columns["UsuarioId"].Visible = false;
                    dgvCitas.Columns["CreadoEn"].Visible = false;
                }

                dgvCitas.ClearSelection();
                _citaSeleccionadaId = null;
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar citas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeshabilitarCamposPersonales()
        {
            txtDocumento.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtTelefono.Enabled = false;
            txtCorreo.Enabled = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Si selecciona fecha anterior a hoy, corregir automáticamente
            if (dtpFechaHora.Value < DateTime.Today)
            {
                MessageBox.Show("No puede seleccionar fechas pasadas. Se ha seleccionado hoy.",
                              "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFechaHora.Value = DateTime.Today;
                return;
            }

            CargarHorasDisponibles(dtpFechaHora.Value);

        }

        private void CargarHorasDisponibles(DateTime fecha)
        {
            cmbHora.Items.Clear();
            cmbHora.Text = "";

            string fechaStr = fecha.ToString("yyyy-MM-dd");

            try
            {
                List<string> citasOcupadas = new List<string>();

                // MANEJO SEGURO: Verificar si el servicio puede obtener citas
                try
                {
                    citasOcupadas = _citaService.ObtenerCitasPorFecha(fechaStr)
                                                .Select(c => c.Hora)
                                                .ToList();
                }
                catch
                {
                    // Si hay error, asumir que no hay citas ocupadas
                    citasOcupadas = new List<string>();
                }

                // Generar horas disponibles
                for (TimeSpan hora = _horaInicio; hora < _horaFin; hora = hora.Add(_intervalo))
                {
                    string horaFormato12 = DateTime.Today.Add(hora).ToString("hh:mm tt");

                    if (!citasOcupadas.Contains(horaFormato12))
                    {
                        cmbHora.Items.Add(horaFormato12);
                    }
                }

                if (cmbHora.Items.Count == 0)
                {
                    cmbHora.Text = "Sin horarios disponibles";
                    cmbHora.Enabled = false;
                }
                else
                {
                    cmbHora.Enabled = true;
                    cmbHora.SelectedIndex = 0;
                }

                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar horarios: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FrmCitas_Load(object sender, EventArgs e)
        {
            try
            {
                // Mostrar datos del usuario
                CargarDatosUsuario();
                DeshabilitarCamposPersonales();

                // Configurar controles de cita
                dtpFechaHora.MinDate = DateTime.Today;
                dtpFechaHora.Value = DateTime.Today;

                // Validar fines de semana
                dtpFechaHora.ValueChanged += ValidarFinDeSemana;

                // Llenar motivos
                LlenarMotivos();

                CargarHorasDisponibles(DateTime.Today);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarMotivos()
        {
            cmbMotivo.Items.AddRange(new string[]
            {
                "Chequeo general",
                "Dolor de cabeza",
                "Consulta por fiebre",
                "Dolor estomacal",
                "Control de presión"
            });
        }

        private void CargarDatosUsuario()
        {
            txtDocumento.Text = _usuarioActual._id.ToString();
            txtNombre.Text = _usuarioActual.nombreUsuario;
            txtApellido.Text = _usuarioActual.apellidoUsuario;
            txtTelefono.Text = _usuarioActual.telefonoUsuario;
            txtCorreo.Text = _usuarioActual.emailUsuario;
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirmar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCamposNuevaCita()) return;

                string fecha = dtpFechaHora.Value.ToString("yyyy-MM-dd");
                string hora = cmbHora.SelectedItem?.ToString();

                // MEJORAR: Verificar disponibilidad (descomentar y usar)
                if (!_citaService.VerificarDisponibilidad(fecha, hora))
                {
                    MessageBox.Show("Esta hora ya no está disponible. Por favor seleccione otra.",
                                  "Horario ocupado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarHorasDisponibles(dtpFechaHora.Value);
                    return;
                }

                // MEJORAR: Usar el servicio
                string nuevoId = _citaService.GenerarNuevoId();

                var nuevaCita = new Citas
                {
                   
                    _id = ObjectId.GenerateNewId(),                   
                    UsuarioId = _usuarioActual._id.ToString(),
                    Motivo = cmbMotivo.Text.Trim(),
                    Fecha = fecha,
                    Hora = hora,
                    Estado = "Pendiente",
                    CreadoEn = DateTime.UtcNow
                };

                _citaService.InsertarCita(nuevaCita);
                MessageBox.Show("✅ Cita registrada correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCamposNuevaCita();
                CargarCitasUsuario(_usuarioActual._id.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la cita: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
         

        private bool ValidarCamposNuevaCita()
        {
            if (cmbMotivo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un motivo de consulta.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMotivo.Focus();
                return false;
            }

            if (cmbHora.SelectedItem == null || cmbHora.Text == "Sin horarios disponibles")
            {
                MessageBox.Show("Seleccione una hora disponible.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbHora.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCamposNuevaCita()
        {
            cmbMotivo.SelectedIndex = -1;
            dtpFechaHora.Value = DateTime.Today;
            CargarHorasDisponibles(DateTime.Today);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvCitas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una cita para editar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvCitas.SelectedRows[0];
            _citaSeleccionadaId = fila.Cells["_id"].Value?.ToString();

            // MEJORAR: Cargar datos en los controles
            cmbMotivo.Text = fila.Cells["Motivo"].Value?.ToString();

            if (DateTime.TryParse(fila.Cells["Fecha"].Value?.ToString(), out DateTime fecha))
            {
                dtpFechaHora.Value = fecha;
            }

            cmbHora.Text = fila.Cells["Hora"].Value?.ToString();

            // MEJORAR: Actualizar estado de botones
            ActualizarEstadoBotones();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_citaSeleccionadaId))
            {
                MessageBox.Show("Seleccione una cita para cancelar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MEJORAR: Agregar confirmación
            var confirm = MessageBox.Show("¿Está seguro de que desea cancelar esta cita?",
                                        "Confirmar Cancelación",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2);

            if (confirm == DialogResult.Yes)
            {
                _citaService.CancelarCita(_citaSeleccionadaId);
                MessageBox.Show("✅ Cita cancelada correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarCitasUsuario(_usuarioActual._id.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_citaSeleccionadaId))
            {
                MessageBox.Show("Seleccione una cita para actualizar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCamposNuevaCita()) return;

            // MEJORAR: Usar el servicio
            _citaService.ActualizarCita(
                _citaSeleccionadaId,
                cmbMotivo.Text.Trim(),
                dtpFechaHora.Value.ToString("yyyy-MM-dd"),
                cmbHora.Text
            );

            MessageBox.Show("Cita actualizada correctamente.", "Éxito",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarCitasUsuario(_usuarioActual._id.ToString());

        }

        private void dgvCitas_MouseClick(object sender, MouseEventArgs e)
        {
            dgvCitas.DefaultCellStyle.SelectionBackColor = Color.MediumPurple; // Fondo al seleccionar
            dgvCitas.DefaultCellStyle.SelectionForeColor = Color.White;        // Texto al seleccionar

        }

        private void dgvCitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvCitas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCitas.Columns.Contains("_id") && dgvCitas.SelectedRows.Count > 0)
            {
                 var fila = dgvCitas.SelectedRows[0];
                _citaSeleccionadaId = fila.Cells["_id"].Value?.ToString();
            }
            else
            {
                _citaSeleccionadaId = null;
            }

            ActualizarEstadoBotones();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Form1 F = new Form1();
            F.Show();
            this.Hide();
        }
    }
}
