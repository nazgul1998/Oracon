using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Entidades
{
    public class Usuario
    {
        [Required]
        public int idUsuario { get; set; }

        [Required]
        public string nombres { get; set; }

        [Required]
        public string apellidos { get; set; }

        [Required]
        public DateTime fechaNac { get; set; }

        [Required]
        public string correo { get; set; }

        [Required]
        public string passwd { get; set; }

        [Required]
        public string usuario { get; set; }
        public List<Entrega> entregas { get; set; }

        public List<Matricula> cursos { get; set; }         
    }
}
