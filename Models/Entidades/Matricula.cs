using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Matricula
    {
        public int idMatricula { get; set; }
        public int idUsuario { get; set; }
        public int idCurso { get; set; }
        public DateTime fechaMatricula { get; set; }
        public string estadoMatricula { get; set; }
        public string codigoMatricula { get; set; }
        public decimal promedio { get; set; }


        public Usuario usuario { get; set; }
        public Curso curso { get; set; }
        
    }
}
