using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Etiqueta
    {
        public int idEtiqueta { get; set; }
        public int idCategoria { get; set; }
        public int idCurso { get; set; }

        public Categoria categoria { get; set; }
        public Curso curso { get; set; }
    }
}
