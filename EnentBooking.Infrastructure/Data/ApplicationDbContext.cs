using EventBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventBooking.Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Event> Events { get; set; }
		public DbSet<Booking> Bookings { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Event Configuration
			builder.Entity<Event>(entity =>
			{
				entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Description).IsRequired();
				entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Venue).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
				entity.Property(e => e.ImageUrl).HasMaxLength(500);
			});

			// Booking Configuration
			builder.Entity<Booking>(entity =>
			{
				entity.Property(b => b.BookingDate).IsRequired();
				entity.Property(b => b.TicketCount).IsRequired();

				// Define relationships
				entity.HasOne(b => b.Event)
					  .WithMany(e => e.Bookings)
					  .HasForeignKey(b => b.EventId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(b => b.User)
					  .WithMany(u => u.Bookings)
					  .HasForeignKey(b => b.UserId)
					  .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
