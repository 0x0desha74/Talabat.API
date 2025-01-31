using Microsoft.EntityFrameworkCore;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Configurations;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new OrderConfigurations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //Apply all Configurations file in one line


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands{ get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        


    }
}
