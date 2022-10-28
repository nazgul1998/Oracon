using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class MatriculaMap : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matricula");
            builder.HasKey(o=>o.idMatricula);

            builder.HasOne(o => o.curso).WithMany(o => o.matriculados).HasForeignKey(o=>o.idCurso);
            builder.HasOne(o => o.usuario).WithMany(o => o.cursos).HasForeignKey(o=>o.idUsuario);
            
        }
    }
}
