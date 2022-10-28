using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Curso
    {
        public int idCurso { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string videoPresentacion { get; set; }
        public decimal precio { get; set; }
        public string estado { get; set; }
        public int idProfesor { get; set; }


        public List<Etiqueta> etiquetas { get; set; }
        public Profesor profesor { get; set; }
        public List<Matricula> matriculados { get; set; }
        public List<Modulo> modulos { get; set; }

    }
}
