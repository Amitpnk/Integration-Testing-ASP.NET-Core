using Bogus;
using CustomerProject.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerProject.IntegrationTest.Fixtures
{
    internal class DataFixture
    {
        public static List<Customers> GetCustomers(int count, bool useNewSeed = false)
        {
            return GetCustomerFaker(useNewSeed).Generate(count);
        }
        public static Customers GetCustomer(bool useNewSeed = false)
        {
            return GetCustomers(1, useNewSeed)[0];
        }

        private static Faker<Customers> GetCustomerFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Customers>()
                .RuleFor(t => t.CustomerId, o => 0)
                .RuleFor(t => t.CustomerCode, (faker, t) => faker.Random.AlphaNumeric(4))
                .RuleFor(t => t.CustomerName, (faker, t) => faker.Name.FullName())
                .UseSeed(seed);
        }
    }
}
