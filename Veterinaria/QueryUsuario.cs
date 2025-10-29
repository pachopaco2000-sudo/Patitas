using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Veterinaria
{
    public class QueryUsuario
    {
        private IMongoDatabase _database;
        private IMongoCollection<Usuarios> _usuariosCollection;

        public QueryUsuario()
        {
            _database = ConexionMongo.ObtenerConexion();
            _usuariosCollection = _database.GetCollection<Usuarios>("Usuarios");
        }

        // MÉTODO PARA BUSCAR POR NÚMERO DE DOCUMENTO (Login)
        public Usuarios ObtenerUsuario(string numeroDocumento)
        {
            try
            {
                var filter = Builders<Usuarios>.Filter.Eq(u => u.numerodocumento, numeroDocumento);
                return _usuariosCollection.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Task.Run(() =>
                    MessageBox.Show($"Error al obtener usuario: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                );
                return null;
            }
        }

        // MÉTODO PARA BUSCAR POR EMAIL (Recuperación de contraseña)
        public Usuarios ObtenerUsuarioPorEmail(string email)
        {
            try
            {
                var filter = Builders<Usuarios>.Filter.Eq(u => u.emailUsuario, email);
                return _usuariosCollection.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Task.Run(() =>
                    MessageBox.Show($"Error al obtener Usuarios: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                );
                return null;
            }
        }

        public bool ActualizarContraseña(string email, string nuevaContraseña)
        {
            try
            {
                var filter = Builders<Usuarios>.Filter.Eq(u => u.emailUsuario, email);
                var update = Builders<Usuarios>.Update
                    .Set(u => u.contraseña, nuevaContraseña);

                var result = _usuariosCollection.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Task.Run(() =>
                    MessageBox.Show($"Error al actualizar contraseña: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                );
                return false;
            }
        }

        // MÉTODO PARA VERIFICAR CREDENCIALES (alternativa)
        public Usuarios VerificarCredenciales(string numeroDocumento, string contraseña)
        {
            try
            {
                var filter = Builders<Usuarios>.Filter.And(
                    Builders<Usuarios>.Filter.Eq(u => u.numerodocumento, numeroDocumento),
                    Builders<Usuarios>.Filter.Eq(u => u.contraseña, contraseña)
                );
                return _usuariosCollection.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar credenciales: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}