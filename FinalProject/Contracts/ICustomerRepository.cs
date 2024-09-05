using FinalProject.DTOs;
using FinalProject.Models;

namespace FinalProject.Contracts
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer> CreateCustomerAsync(CustomerDto customerDTO);
        Task UpdateCustomerAsync(Guid id, CustomerDto customerDTO);
        Task DeleteCustomerAsync(Guid id);
    }
}
