using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class EnlaceMap : IEntityTypeConfiguration<Enlace>
    {
        public void Configure(EntityTypeBuilder<Enlace> builder)
        {
            builder.ToTable("Enlace");
            builder.HasKey(o=>o.idEnlace);

            builder.HasOne(o => o.modulo).WithMany(o => o.enlaces).HasForeignKey(o => o.idModulo);

        }
    }
}
