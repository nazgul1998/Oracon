using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class DocumentoMap : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            builder.ToTable("Documento");
            builder.HasKey(o=>o.idDocumento);

            builder.HasOne(o => o.modulo).WithMany(o => o.documentos).HasForeignKey(o=>o.idModulo);
            
        }
    }
}
