using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppReserve.Domain;
using AppReserve.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppReserve.Infrastructure.Persistence.Configs
{
    public class ReservationConfig : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.EndDate)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne<User>()
              .WithMany() 
              .HasForeignKey(r => r.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Space>()
             .WithMany() 
             .HasForeignKey(r => r.SpaceId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasCheckConstraint("CH_Reservations_StartBeforeEnd", "StartDate < EndDate");

            builder.HasIndex(r => new { r.SpaceId, r.StartDate, r.EndDate });
            builder.HasIndex(r => new { r.UserId, r.StartDate, r.EndDate });
        }
    }
}
