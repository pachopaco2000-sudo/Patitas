using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Veterinaria
{
    public class Usuarios
    {

        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("nombreUsuario")]
        public string nombreUsuario { get; set; }

        [BsonElement("numerodocumento")]
        public string numerodocumento { get; set; }

        [BsonElement("apellidoUsuario")]
        public string apellidoUsuario { get; set; }

        [BsonElement("telefonoUsuario")]
        public string telefonoUsuario { get; set; }

        [BsonElement("emailUsuario")]
        public string emailUsuario { get; set; }

        [BsonElement("contraseña")]
        public string contraseña { get; set; }

        [BsonElement("rolUsuario")]
        public string rolUsuario { get; set; }
    }
}


