using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NexoraERP.Infrastructure.Persistence.Master;

public sealed class MasterDbContextDesignTimeFactory : IDesignTimeDbContextFactory<MasterDbContext>
{
    public MasterDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MasterDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=NexoraERP_Master;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new MasterDbContext(options);
    }
}
