using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Prism.DAL
{
    public class PrismContextFactory : IDesignTimeDbContextFactory<PrismContext>
    {
        public PrismContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("SqlConnection");
            var optionsBuilder = new DbContextOptionsBuilder<PrismContext>();
            // Provide your connection string here
            optionsBuilder.UseSqlServer(connectionString);

            return new PrismContext(optionsBuilder.Options);
        }
    }
}
