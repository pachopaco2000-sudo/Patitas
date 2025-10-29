using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Veterinaria
{
    public class Usuario
    {

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string _id { get; set; }

        [BsonElement("nombre")]
        public string nombre { get; set; }

        [BsonElement("apellidos")]
        public string apellidos { get; set; }

        [BsonElement("tipoDocumento")]
        public string tipoDocumento { get; set; }

        [BsonElement("numeroDocumento")]
        public string numeroDocumento { get; set; }

        [BsonElement("telefono")]
        public string telefono { get; set; }

        [BsonElement("correo")]
        public string correo { get; set; }

        [BsonElement("contraseña")]
        public string contraseña { get; set; }

        [BsonElement("rol")]
        public string rol { get; set; }
    }
}


