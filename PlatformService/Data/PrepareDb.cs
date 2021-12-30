using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepareDb
    {
        public static void Configure(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext dbContext)
        {
            if(!dbContext.Platforms.Any())
            {
                Console.WriteLine("Seeding Data......");

                dbContext.Platforms.AddRange(
                    new List<Platform>(){
                        new Platform{Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                        new Platform{Name = "Sql Server", Publisher = "Microsoft", Cost = "License"},
                        new Platform{Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"},
                    }
                );

                dbContext.SaveChanges();
            }
            else
                Console.WriteLine("Data exists......");
        }
    }
}