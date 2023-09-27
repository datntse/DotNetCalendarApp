using CalendarApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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


		}
	}
}