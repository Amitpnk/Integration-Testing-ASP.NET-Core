using CustomerProject.WebApi.DbContexts;
using CustomerProject.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerProject.WebApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext _context;

        public CustomerService(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customers>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customers> GetCustomerAsync(int Id)
        {
            return await _context.Customers.FindAsync(Id);
        }

        public async Task<bool> AddCustomerAsync(Customers newCustomer)
        {
            _context.Customers.Add(newCustomer);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditCustomerAsync(Customers editedCustomer)
        {
            _context.Update(editedCustomer);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCustomerAsync(int Id)
        {
            var student = await _context.Customers.FindAsync(Id);
            if (student is null)
            {
                return false;
            }
            _context.Customers.Remove(student);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
