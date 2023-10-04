using CalendarApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CalendarApp.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }

		public DbSet<Job> Jobs { get; set; }

		
		protected override void OnModelCreating(ModelBuilder builder)
		{

			base.OnModelCreating(builder);
			// Bỏ tiền tố AspNet của các bảng: mặc định
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var tableName = entityType.GetTableName();
				if (tableName.StartsWith("AspNet"))
				{
					entityType.SetTableName(tableName.Substring(6));
				}
			}
			builder.Entity<Job>(entity =>
			{
				entity.HasNoKey();
				entity.ToTable("Job", "HangFire");
				entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt")
					.HasFilter("([ExpireAt] IS NOT NULL)");

				entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName")
					.HasFilter("([StateName] IS NOT NULL)");

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");

				entity.Property(e => e.ExpireAt).HasColumnType("datetime");

				entity.Property(e => e.StateName).HasMaxLength(20);
			});


		}
	}
}