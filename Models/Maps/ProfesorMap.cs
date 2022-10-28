using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class ProfesorMap : IEntityTypeConfiguration<Profesor>
    {
        public void Configure(EntityTypeBuilder<Profesor> builder)
        {
            builder.ToTable("Profesor");
            builder.HasKey(o=>o.idProfesor);

            builder.HasMany(o => o.cursos).WithOne(o=>o.profesor).HasForeignKey(o => o.idCurso);
        }
    }
}
