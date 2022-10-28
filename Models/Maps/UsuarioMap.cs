using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(o=>o.idUsuario);

            builder.HasMany(o => o.cursos).WithOne(o => o.usuario).HasForeignKey(o=>o.idCurso);
            builder.HasMany(o => o.entregas).WithOne(o => o.usuario).HasForeignKey(o=>o.idEntrega);


        }
    }
}
