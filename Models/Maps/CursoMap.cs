using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class CursoMap : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Curso");
            builder.HasKey(o=>o.idCurso);

            //relacion de cursos y profesor
            builder.HasOne(o => o.profesor).WithMany(o=>o.cursos).HasForeignKey(o=>o.idProfesor);

            //relacion de cursos y etiquetas
            builder.HasMany(o => o.etiquetas).WithOne(o=>o.curso).HasForeignKey(o=>o.idEtiqueta);

            //relacion de cursos y matriculados
            builder.HasMany(o => o.matriculados).WithOne(o => o.curso).HasForeignKey(o=> o.idMatricula);

            //relacion de cursos y contenido
            builder.HasMany(o => o.modulos).WithOne(o => o.curso).HasForeignKey(o => o.idModulo);

        }
    }
}
