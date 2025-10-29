using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Veterinaria
{
    public class CitaService
    {
        private readonly IMongoCollection<Cita> _citasCollection;

        public CitaService(IMongoDatabase database)
        {
            _citasCollection = database.GetCollection<Cita>("Citas");
        }

        public List<Cita> ObtenerCitasUsuario(string usuarioId)
        {
            try
            {
                var filter = Builders<Cita>.Filter.Eq(c => c.UsuarioId, usuarioId);
                var citas = _citasCollection.Find(filter).ToList();
                return citas ?? new List<Cita>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo citas: {ex.Message}");
                return new List<Cita>();
            }
        }

        public bool VerificarDisponibilidad(string fecha, string hora)
        {
            try
            {
                var citaExistente = _citasCollection.Find(c => c.Fecha == fecha && c.Hora == hora).FirstOrDefault();
                return citaExistente == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verificando disponibilidad: {ex.Message}");
                return false;
            }
        }

        public string GenerarNuevoId()
        {
            try
            {
                var ultimaCita = _citasCollection.Find(_ => true)
                                               .SortByDescending(c => c._id)
                                               .FirstOrDefault();

                if (ultimaCita == null)
                    return "C001";

                string numeroStr = new string(ultimaCita._id.Where(char.IsDigit).ToArray());
                if (int.TryParse(numeroStr, out int numero))
                {
                    return $"C{(numero + 1):D3}";
                }

                return "C001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generando ID: {ex.Message}");
                return "C001";
            }
        }

        public void InsertarCita(Cita cita)
        {
            try
            {
                _citasCollection.InsertOne(cita);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insertando cita: {ex.Message}");
                throw;
            }
        }

        public void ActualizarCita(string id, string motivo, string fecha, string hora)
        {
            try
            {
                var update = Builders<Cita>.Update
                    .Set(c => c.Motivo, motivo)
                    .Set(c => c.Fecha, fecha)
                    .Set(c => c.Hora, hora)
                    .Set(c => c.Estado, "Reprogramada");

                _citasCollection.UpdateOne(c => c._id == id, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando cita: {ex.Message}");
                throw;
            }
        }

        public void CancelarCita(string id)
        {
            try
            {
                _citasCollection.DeleteOne(c => c._id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cancelando cita: {ex.Message}");
                throw;
            }
        }

        public List<Cita> ObtenerCitasPorFecha(string fecha)
        {
            try
            {
                return _citasCollection.Find(c => c.Fecha == fecha).ToList() ?? new List<Cita>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo citas por fecha: {ex.Message}");
                return new List<Cita>();
            }
        }

        // MÉTODO ADICIONAL: Para obtener cita por ID
        public Cita ObtenerCitaPorId(string id)
        {
            try
            {
                return _citasCollection.Find(c => c._id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo cita por ID: {ex.Message}");
                return null;
            }
        }
    }
}