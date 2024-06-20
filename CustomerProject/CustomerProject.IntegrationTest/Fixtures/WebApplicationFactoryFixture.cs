using CustomerProject.WebApi.DbContexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerProject.IntegrationTest.Fixtures
{
    public class WebApplicationFactoryFixture : IAsyncLifetime
    {
        private const string _connectionString = @$"Data Source=(local);Initial Catalog=UserIntegration;Integrated Security=True;Encrypt=False";

        private WebApplicationFactory<Program> _factory;

        public HttpClient Client { get; private set; }
        public int InitialCustomersCount { get; set; } = 3;

        public WebApplicationFactoryFixture()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(Services =>
                {
                    Services.RemoveAll(typeof(DbContextOptions<CustomerDbContext>));
                    Services.AddDbContext<CustomerDbContext>(options =>
                    {
                        options.UseSqlServer(_connectionString);
                    });
                });
            });
            Client = _factory.CreateClient();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<CustomerDbContext>();

                await cntx.Database.EnsureDeletedAsync();
            }
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<CustomerDbContext>();

                await cntx.Database.EnsureCreatedAsync();

                await cntx.Customers.AddRangeAsync(DataFixture.GetCustomers(InitialCustomersCount));
                await cntx.SaveChangesAsync();
            }
        }
    }
}
