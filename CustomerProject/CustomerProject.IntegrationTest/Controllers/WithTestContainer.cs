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
    public class WithTestContainer : IClassFixture<DockerWebApplicationFactoryFixture>
    {
        private readonly DockerWebApplicationFactoryFixture _factory;
        private readonly HttpClient _client;

        public WithTestContainer(DockerWebApplicationFactoryFixture factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task OnGetStudent_WhenExecuteController_ShouldreturnTheExpecedStudet()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(HttpHelper.Urls.GetAllCustomers);
            var result = await response.Content.ReadFromJsonAsync<List<Customers>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result?.Count.Should().Be(_factory.InitialStudentsCount);
            result.Should()
                .BeEquivalentTo(DataFixture.GetCustomers(_factory.InitialStudentsCount), options => options.Excluding(t => t.CustomerId));
        }

        [Fact]
        public async Task OnAddStudent_WhenExecuteController_ShouldStoreInDb()
        {
            // Arrange
            var newStudent = DataFixture.GetCustomer(true);

            // Act
            var request = await _client.PostAsync(HttpHelper.Urls.AddCustomer, HttpHelper.GetJsonHttpContent(newStudent));
            var response = await _client.GetAsync($"{HttpHelper.Urls.GetCustomer}/{_factory.InitialStudentsCount + 1}");
            var result = await response.Content.ReadFromJsonAsync<Customers>();

            // Assert
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


            result?.CustomerCode.Should().Be(newStudent.CustomerCode);
            result?.CustomerName.Should().Be(newStudent.CustomerName);
        }
    }
}
