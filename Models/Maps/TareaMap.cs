using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class TareaMap : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> builder)
        {
            builder.ToTable("Tarea");
            builder.HasKey(o=>o.idTarea);

            builder.HasOne(o => o.modulo).WithMany(o => o.tareas).HasForeignKey(o=>o.idModulo);
            builder.HasMany(o => o.entregas).WithOne(o => o.tarea).HasForeignKey(o => o.idEntrega);
        }
    }
}
