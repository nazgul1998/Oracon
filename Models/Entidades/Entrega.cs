using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Entrega
    {
        public int idEntrega { get; set; }
        public int idTarea { get; set; }
        public byte[] documento { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }
        public int idUsuario { get; set; }
        public DateTime fechaEntrega { get; set; }

        public Tarea tarea { get; set; }
        public Usuario usuario { get; set; }
    }
}
