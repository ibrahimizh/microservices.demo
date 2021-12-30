using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext dbContext;

        public PlatformRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void CreatePlatform(Platform platform)
        {
            if(platform == null)
                throw new ArgumentNullException();

            dbContext.Platforms.Add(platform);
        }

        public Platform GetPlatform(int id)
        {
            return dbContext.Platforms.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return dbContext.Platforms;
        }

        public bool SaveChanges()
        {
            return dbContext.SaveChanges() >= 0;
        }
    }
}