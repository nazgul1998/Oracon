using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Pago
    {
        public int idPago { get; set; }
        public int idMatricula { get; set; }
        public string estado { get; set; }
        public DateTime fechaPago { get; set; }
        public decimal monto { get; set; }


        public Matricula matricula { get; set; }

    }
}
