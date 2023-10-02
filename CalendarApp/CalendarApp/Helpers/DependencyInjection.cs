using Microsoft.EntityFrameworkCore.Query.Internal;
using Quartz;

namespace CalendarApp.Helpers
{
	public static class DependencyInjection
	{
		public static void AddInfrastructure(this IServiceCollection services)
		{
			services.AddQuartz(options =>
			{
				options.UseMicrosoftDependencyInjectionJobFactory();

			});


			services.AddQuartzHostedService(options =>
			{
				options.WaitForJobsToComplete = true; // chờ jobs complete thì app mới shutdown
			});

		}

	}
}
