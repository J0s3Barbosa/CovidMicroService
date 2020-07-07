using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using System;

namespace Services.Infra.Map
{
    public class Covid19Configurations : IEntityTypeConfiguration<Covid19>
    {
        public void Configure(EntityTypeBuilder<Covid19> builder)
        {
            builder.Property(x => x.Id)
                .IsRequired(true);

            builder.Property(x => x.Confirmados)
                .IsRequired(true);

            builder.Property(x => x.DadosDoDia)
                .IsRequired(true);

            builder.Property(x => x.Local)
               .IsRequired(true);

            builder.Property(x => x.Mortes)
               .IsRequired(false);

            builder.Property(x => x.Recuperados)
               .IsRequired(false);


            builder.ToTable("Covid19").HasKey(x => x.Id);

            builder.HasData(new Covid19()
            {
                Id = Guid.NewGuid().ToString(),
                Confirmados = "00",
                DadosDoDia = "00",
                Local = DateTime.Now.ToShortDateString(),
                Mortes = "00",
                Recuperados = "00"
            });
        }

    }
}
