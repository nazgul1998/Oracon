using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Profesor
    {
        public int idProfesor { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public DateTime fechaNac { get; set; }
        public string correo { get; set; }
        public string passwd { get; set; }
        public string usuario { get; set; }
        public string celular { get; set; }
        public string dni { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public byte[] foto { get; set; }

        public List<Curso> cursos { get; set; }
    }
}
