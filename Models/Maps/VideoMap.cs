using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models.Maps
{
    public class VideoMap : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Video");
            builder.HasKey(o=>o.idVideo);

            builder.HasOne(o => o.modulo).WithMany(o => o.videos).HasForeignKey(o=>o.idModulo);
        }
    }
}
