using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Categoria
    {
        public int idCategoria { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public List<Etiqueta> etiquetas { get; set; }

    }
}
