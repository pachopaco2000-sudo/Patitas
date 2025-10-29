using MongoDB.Driver;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Veterinaria
{
    public static class ConexionMongo
    {
        private static readonly string Puerto = "mongodb://localhost:27017";
        private static readonly string BasedeDatos = "Veterinaria";
        private static IMongoDatabase _database;

        public static IMongoDatabase ObtenerConexion()
        {
            try
            {
                if (_database != null) return _database;

                var client = new MongoClient(Puerto);

                // 🔍 Forzar conexión con el servidor antes de acceder a la base
                var databases = client.ListDatabaseNames().ToList();

                _database = client.GetDatabase(BasedeDatos);

                // Opcional: ver colecciones existentes
                var collections = _database.ListCollectionNames().ToList();

                return _database;
            }
            catch (Exception ex)
            {
                string mensajeError =
                    $"Error conectando a MongoDB:\n{ex.Message}\n\n" +
                    "Verifica:\n" +
                    "1. Que MongoDB esté instalado\n" +
                    "2. Que el servicio esté ejecutándose\n" +
                    "3. Que la base de datos exista en Compass";

                MessageBox.Show(mensajeError, "Error de Conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool EstaConectado()
        {
            try { return ObtenerConexion() != null; }
            catch { return false; }
        }

        public static string ObtenerInfoConexion()
        {
            string estado = EstaConectado() ? "✅ Conectado" : "❌ Desconectado";
            return $"Modo: Local (Compass)\nEstado: {estado}\nBase de datos: {BasedeDatos}";
        }
    }
}
