using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Veterinaria
{
    public class ModoDemo
    {
        private static List<Usuarios> _usuariosDemo = new List<Usuarios>
        {
            new Usuarios 
            {
                _id = "U001",
                numeroDocumento = "12345678",
                nombre = "Juan",
                apellidos = "Pérez López",
                correo = "juan@demo.com",
                contraseña = "123456",
                telefono = "3001234567",
                rol = "Paciente"
            },
            new Usuarios
            {
                _id = "U002",
                numeroDocumento = "87654321",
                nombre = "María",
                apellidos = "García Rodríguez",
                correo = "maria@demo.com",
                contraseña = "123456",
                telefono = "3007654321",
                rol = "Medico"
            },
            new Usuarios
            {
                _id = "U003",
                numeroDocumento = "11223344",
                nombre = "Admin",
                apellidos = "Sistema",
                correo = "admin@demo.com",
                contraseña = "admin123",
                telefono = "3001112233",
                rol = "admin"
            }
        };

        public static bool EstaActivo { get; set; } = false;

        public static Usuarios ObtenerUsuarioDemo(string numeroDocumento)
        {
            return _usuariosDemo.Find(u => u.numeroDocumento == numeroDocumento);
        }

        public static bool ValidarUsuarioDemo(string numeroDocumento, string contraseña)
        {
            var usuario = ObtenerUsuarioDemo(numeroDocumento);
            return usuario != null && usuario.contraseña == contraseña;
        }
    }
}