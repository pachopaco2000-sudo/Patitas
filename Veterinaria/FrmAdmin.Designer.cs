namespace Veterinaria
{
    partial class FrmAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnMostrar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotalCitas = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCitasPendientes = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblCitasConfirmadas = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblCitasCompletadas = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(242)))), ((int)(((byte)(249)))));
            this.label12.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(43, -54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(182, 23);
            this.label12.TabIndex = 19;
            this.label12.Text = "Gestión de Citas 👨‍👨‍👧‍👦‍❤️‍";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnMostrar);
            this.groupBox5.Controls.Add(this.btnBuscar);
            this.groupBox5.Controls.Add(this.txtBuscar);
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Controls.Add(this.groupBox3);
            this.groupBox5.Controls.Add(this.groupBox4);
            this.groupBox5.Controls.Add(this.dgvUsuarios);
            this.groupBox5.Location = new System.Drawing.Point(30, 119);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(782, 566);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            // 
            // btnMostrar
            // 
            this.btnMostrar.BackColor = System.Drawing.Color.White;
            this.btnMostrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMostrar.Location = new System.Drawing.Point(372, 185);
            this.btnMostrar.Name = "btnMostrar";
            this.btnMostrar.Size = new System.Drawing.Size(90, 25);
            this.btnMostrar.TabIndex = 21;
            this.btnMostrar.Text = "Mostrar ";
            this.btnMostrar.UseVisualStyleBackColor = false;
            this.btnMostrar.Click += new System.EventHandler(this.btnMostrar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.White;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.Location = new System.Drawing.Point(52, 185);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 25);
            this.btnBuscar.TabIndex = 20;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.btnBuscar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnBuscar_KeyPress);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(139, 185);
            this.txtBuscar.Multiline = true;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(188, 25);
            this.txtBuscar.TabIndex = 17;
            this.txtBuscar.DoubleClick += new System.EventHandler(this.txtBuscar_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTotalCitas);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(75, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 104);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // lblTotalCitas
            // 
            this.lblTotalCitas.AutoSize = true;
            this.lblTotalCitas.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblTotalCitas.Location = new System.Drawing.Point(50, 75);
            this.lblTotalCitas.Name = "lblTotalCitas";
            this.lblTotalCitas.Size = new System.Drawing.Size(13, 13);
            this.lblTotalCitas.TabIndex = 7;
            this.lblTotalCitas.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(20, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total de Citas";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCitasPendientes);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(235, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(112, 104);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // lblCitasPendientes
            // 
            this.lblCitasPendientes.AutoSize = true;
            this.lblCitasPendientes.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCitasPendientes.Location = new System.Drawing.Point(57, 75);
            this.lblCitasPendientes.Name = "lblCitasPendientes";
            this.lblCitasPendientes.Size = new System.Drawing.Size(13, 13);
            this.lblCitasPendientes.TabIndex = 6;
            this.lblCitasPendientes.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Citas Pendientes";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblCitasConfirmadas);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(396, 37);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(112, 104);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            // 
            // lblCitasConfirmadas
            // 
            this.lblCitasConfirmadas.AutoSize = true;
            this.lblCitasConfirmadas.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCitasConfirmadas.Location = new System.Drawing.Point(54, 75);
            this.lblCitasConfirmadas.Name = "lblCitasConfirmadas";
            this.lblCitasConfirmadas.Size = new System.Drawing.Size(13, 13);
            this.lblCitasConfirmadas.TabIndex = 5;
            this.lblCitasConfirmadas.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(15, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Citas Confirmadas";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblCitasCompletadas);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(561, 37);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(112, 104);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // lblCitasCompletadas
            // 
            this.lblCitasCompletadas.AutoSize = true;
            this.lblCitasCompletadas.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCitasCompletadas.Location = new System.Drawing.Point(57, 75);
            this.lblCitasCompletadas.Name = "lblCitasCompletadas";
            this.lblCitasCompletadas.Size = new System.Drawing.Size(13, 13);
            this.lblCitasCompletadas.TabIndex = 4;
            this.lblCitasCompletadas.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(12, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Citas Completadas";
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.Location = new System.Drawing.Point(22, 246);
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.Size = new System.Drawing.Size(688, 237);
            this.dgvUsuarios.TabIndex = 12;
            this.dgvUsuarios.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUsuarios_CellMouseDoubleClick);
            this.dgvUsuarios.SelectionChanged += new System.EventHandler(this.dgvUsuarios_SelectionChanged);
            // 
            // FrmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(242)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(864, 721);
            this.ControlBox = false;
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox5);
            this.Name = "FrmAdmin";
            this.Text = " ";
            this.Load += new System.EventHandler(this.FrmAdmin_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTotalCitas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblCitasPendientes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblCitasConfirmadas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblCitasCompletadas;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnMostrar;
    }
}