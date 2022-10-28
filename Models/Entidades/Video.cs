using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Video
    {
        public int idVideo { get; set; }
        public int idModulo { get; set; }
        public string link { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }

        public Modulo modulo { get; set; }
    }
}
