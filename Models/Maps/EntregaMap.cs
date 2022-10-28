using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class EntregaMap : IEntityTypeConfiguration<Entrega>
    {
        public void Configure(EntityTypeBuilder<Entrega> builder)
        {
            builder.ToTable("Entrega");
            builder.HasKey(p=>p.idEntrega);

            builder.HasOne(o => o.tarea).WithMany(o => o.entregas).HasForeignKey(o=>o.idTarea);
            builder.HasOne(o => o.usuario).WithMany(o=>o.entregas).HasForeignKey(o=>o.idUsuario);
        }
    }
}
