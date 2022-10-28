using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class ModuloMap : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.ToTable("Modulo");
            builder.HasKey(o=>o.idModulo);


            builder.HasOne(o => o.curso).WithMany(o => o.modulos).HasForeignKey(o=>o.idCurso);
            builder.HasMany(o => o.videos).WithOne(o => o.modulo).HasForeignKey(o=>o.idVideo);
            builder.HasMany(o => o.documentos).WithOne(o => o.modulo).HasForeignKey(o => o.idDocumento);
            builder.HasMany(o => o.enlaces).WithOne(o => o.modulo).HasForeignKey(o => o.idEnlace);
            builder.HasMany(o => o.tareas).WithOne(o => o.modulo).HasForeignKey(o => o.idTarea);
        }
    }
}
