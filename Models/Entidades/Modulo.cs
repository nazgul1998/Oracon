using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Modulo
    {
        public int idModulo { get; set; }
        public int idCurso { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }


        public Curso curso { get; set; }
        public List<Video> videos { get; set; }
        public List<Documento> documentos { get; set; }
        public List<Enlace> enlaces { get; set; }
        public List<Tarea> tareas { get; set; }



    }
}
