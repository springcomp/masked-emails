using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class MaskedEmailsDbContextFactory : IMaskedEmailsDbContextFactory
    {
        private readonly IConfiguration configuration_;
        public MaskedEmailsDbContextFactory(IConfiguration configuration)
        {
            configuration_ = configuration;
        }
        public MaskedEmailsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MaskedEmailsDbContext>();
            optionsBuilder.UseSqlite(configuration_.GetConnectionString("DefaultConnection"));

            return new MaskedEmailsDbContext(optionsBuilder.Options);
        }
    }
}