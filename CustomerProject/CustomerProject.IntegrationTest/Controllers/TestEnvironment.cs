using CustomerProject.IntegrationTest.Fixtures;
using CustomerProject.IntegrationTest.Helper;
using CustomerProject.WebApi.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CustomerProject.IntegrationTest.Controllers
{
    public class TestEnvironemnt : IClassFixture<WebApplicationFactoryFixture>
    {
        private readonly WebApplicationFactoryFixture _factory;

        public TestEnvironemnt(WebApplicationFactoryFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task OnGetCustomer_WhenExecuteController_ShouldreturnTheExpecedStudet()
        {
            // Act
            var response = await _factory.Client.GetAsync(HttpHelper.Urls.GetAllCustomers);
            var result = await response.Content.ReadFromJsonAsync<List<Customers>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result?.Count.Should().Be(_factory.InitialCustomersCount+1);
            //result.Should()
            //    .BeEquivalentTo(DataFixture.GetCustomers(_factory.InitialCustomersCount), options => options.Excluding(t => t.CustomerId));
        }

        [Fact]
        public async Task OnAddCustomer_WhenExecuteController_ShouldStoreInDb()
        {
            // Arrange
            var newCustomer = DataFixture.GetCustomer(true);

            // Act
            var request = await _factory.Client.PostAsync(HttpHelper.Urls.AddCustomer, HttpHelper.GetJsonHttpContent(newCustomer));
            var response = await _factory.Client.GetAsync($"{HttpHelper.Urls.GetCustomer}/{_factory.InitialCustomersCount + 1}");
            var result = await response.Content.ReadFromJsonAsync<Customers>();

            // Assert
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result?.CustomerCode.Should().Be(newCustomer.CustomerCode);
            result?.CustomerName.Should().Be(newCustomer.CustomerName);
        }         
    }
}
