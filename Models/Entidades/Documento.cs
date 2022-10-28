using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Documento
    {
        public int idDocumento { get; set; }
        public int idModulo { get; set; }
        public byte[] documento { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }

        public Modulo modulo { get; set; }
    }
}
