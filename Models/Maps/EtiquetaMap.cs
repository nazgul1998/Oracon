using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class EtiquetaMap : IEntityTypeConfiguration<Etiqueta>
    {
        public void Configure(EntityTypeBuilder<Etiqueta> builder)
        {
            builder.ToTable("Etiqueta");
            builder.HasKey(o=>o.idEtiqueta);

            builder.HasOne(o => o.curso).WithMany(o => o.etiquetas).HasForeignKey(o=>o.idCurso);
            builder.HasOne(o => o.categoria).WithMany(o => o.etiquetas).HasForeignKey(o=>o.idCategoria);
        }
    }
}
