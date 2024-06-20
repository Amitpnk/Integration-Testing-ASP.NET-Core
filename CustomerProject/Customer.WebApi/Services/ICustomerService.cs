using CustomerProject.WebApi.Models;

namespace CustomerProject.WebApi.Services;

public interface ICustomerService
{
    Task<bool> AddCustomerAsync(Customers newCustomer);
    Task<bool> DeleteCustomerAsync(int Id);
    Task<bool> EditCustomerAsync(Customers editedCustomer);
    Task<Customers> GetCustomerAsync(int Id);
    Task<List<Customers>> GetCustomersAsync();
}