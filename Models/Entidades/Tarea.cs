using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Tarea
    {
        public int idTarea { get; set; }
        public string nombre { get; set; }
        public string instrucciones { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaMaxima { get; set; }
        public int idModulo { get; set; }


        public Modulo modulo { get; set; }
        public List<Entrega> entregas { get; set; }
    }
}
