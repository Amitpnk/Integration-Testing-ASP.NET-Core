using CustomerProject.IntegrationTest.Helper;
using CustomerProject.WebApi.DbContexts;
using CustomerProject.WebApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CustomerProject.IntegrationTest.Controllers;
public class InMemoryDatabase
{
    private WebApplicationFactory<Program> _factory;

    public InMemoryDatabase()
    {

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<CustomerDbContext>));
                    services.AddDbContext<CustomerDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("test");
                    });
                });
            });
    }

    [Fact]
    public async void OnGetCustomer_WhenExecuteApi_ShouldReturnExpectedCustomer()
    {
        // Arrange

        using (var scope = _factory.Services.CreateScope())
        {
            var scopService = scope.ServiceProvider;
            var dbContext = scopService.GetRequiredService<CustomerDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Customers.Add(new Customers
            {
                CustomerCode = "1001",
                CustomerName= "Amit"
            });
            dbContext.SaveChanges();
        }

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(HttpHelper.Urls.GetAllCustomers);
        var result = await response.Content.ReadFromJsonAsync<List<Customers>>();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        result?.Count.Should().Be(1);
        result?.FirstOrDefault()?.CustomerCode.Should().Be("1001");
        result?.FirstOrDefault()?.CustomerName.Should().Be("Amit");
    }

    [Fact]
    public async Task OnAddCustomer_WhenExecuteController_ShouldStoreInDb()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<CustomerDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
        var client = _factory.CreateClient();
        //var newCustomer = DataFixture.GetCustomer();

        //var httpContent = HttpHelper.GetJsonHttpContent(newCustomer);

        // Act
        //var request = await client.PostAsync(HttpHelper.Urls.AddCustomer, httpContent);
        var response = await client.GetAsync(HttpHelper.Urls.GetAllCustomers);
        var result = await response.Content.ReadFromJsonAsync<List<Customers>>();

        // Assert
        //request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        result?.Count.Should().Be(1);
        result?.FirstOrDefault()?.CustomerCode.Should().Be("1001");
        result?.FirstOrDefault()?.CustomerName.Should().Be("Amit");

        //result?.FirstOrDefault()?.CustomerCode.Should().Be(newCustomer.CustomerCode);
        //result?.FirstOrDefault()?.CustomerName.Should().Be(newCustomer.CustomerName);
    }

}
