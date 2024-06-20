using CustomerProject.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CustomerProject.WebApi.DbContexts;
public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    public DbSet<Customers> Customers { get; set; }
}

