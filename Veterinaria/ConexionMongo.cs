using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Net;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Veterinaria
{
    public static class ConexionMongo
    {
        // Nombre por defecto de tu DB en Atlas (cámbialo si usas otro)
        private const string DefaultDatabaseName = "Patitas";

        // Leer la cadena desde variable de entorno MONGO_CONN si está definida; si no, usar literal de respaldo.
        private static readonly string _connectionString =
            Environment.GetEnvironmentVariable("MONGO_CONN")
            ?? "mongodb+srv://jesus:jesus@patitas.skegh3j.mongodb.net/?appName=Patitas";

        // Cliente singleton, thread-safe (creado a partir de la cadena por defecto)
        private static readonly Lazy<MongoClient> _lazyClient = new Lazy<MongoClient>(() =>
        {
            // Para .NET Framework antiguos, fuerza TLS1.2
            try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { /* ignore */ }

            var settings = MongoClientSettings.FromConnectionString(_connectionString);

            // Opcional: timeouts razonables
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
            settings.ConnectTimeout = TimeSpan.FromSeconds(5);
            settings.MaxConnectionPoolSize = 200;

            return new MongoClient(settings);
        });

        // Si el usuario llama a Connect explícitamente, usamos este cliente en su lugar
        private static MongoClient _manualClient;
        private static string _manualDatabaseName;

        private static MongoClient Client => _manualClient ?? _lazyClient.Value;

        /// <summary>
        /// Permite al código establecer una cadena de conexión manualmente (por ejemplo desde Form1) y el nombre de la BD.
        /// Esto mantiene compatibilidad con el uso anterior de ConexionMongo.Connect(...).
        /// </summary>
        public static void Connect(string connectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Se requiere la cadena de conexión", nameof(connectionString));
            if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentException("Se requiere el nombre de la base de datos", nameof(databaseName));

            try
            {
                // Forzamos TLS1.2
                try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { }

                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
                settings.ConnectTimeout = TimeSpan.FromSeconds(5);

                _manualClient = new MongoClient(settings);
                _manualDatabaseName = databaseName;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo inicializar el cliente MongoDB: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene una base de datos (por defecto "Patitas"), usando la conexión manual si fue establecida.
        /// </summary>
        public static IMongoDatabase GetDatabase(string dbName = DefaultDatabaseName)
        {
            if (string.IsNullOrWhiteSpace(dbName)) dbName = DefaultDatabaseName;
            return Client.GetDatabase(dbName);
        }

        /// <summary>
        /// Método de compatibilidad con el código existente: devuelve la conexión (IMongoDatabase) usando la BD manual si existe.
        /// </summary>
        public static IMongoDatabase ObtenerConexion()
        {
            var dbName = _manualDatabaseName ?? DefaultDatabaseName;
            return GetDatabase(dbName);
        }

        /// <summary>
        /// Obtiene una colección tipada.
        /// </summary>
        public static IMongoCollection<T> GetCollection<T>(string collectionName, string dbName = DefaultDatabaseName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("El nombre de la colección no puede estar vacío.", nameof(collectionName));

            return GetDatabase(dbName).GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Hace ping a Atlas para verificar conectividad (async).
        /// </summary>
        public static async Task<bool> TestConnectionAsync(string dbName = null)
        {
            try
            {
                var db = GetDatabase(dbName ?? (_manualDatabaseName ?? DefaultDatabaseName));
                var command = new BsonDocument("ping", 1);
                await db.RunCommandAsync<BsonDocument>(command);
                return true;
            }
            catch (Exception ex)
            {
                // Mostrar error en UI para facilitar depuración en WinForms
                try
                {
                    MessageBox.Show($"Error conectando a MongoDB:\n{ex.Message}", "MongoDB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
                return false;
            }
        }

        /// <summary>
        /// Versión sincrónica de prueba de conexión (compatibilidad con código existente).
        /// </summary>
        public static bool TestConnection(string dbName = null)
        {
            return TestConnectionAsync(dbName).GetAwaiter().GetResult();
        }
    }
}
