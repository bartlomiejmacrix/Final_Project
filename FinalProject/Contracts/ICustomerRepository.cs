using FinalProject.DTOs;
using FinalProject.Models;

namespace FinalProject.Contracts
{
    /// <summary>
    /// Defines the contract for customer repository operations.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns>The task result contains a list of customers.</returns>
        Task<List<Customer>> GetCustomersAsync();

        /// <summary>
        /// Creates a new customer based on the provided data.
        /// </summary>
        /// <param name="customerDTO">The data transfer object containing customer details to create.</param>
        /// <returns>The task result contains the created customer.</returns>
        Task<Customer> CreateCustomerAsync(CustomerDto customerDTO);

        /// <summary>
        /// Updates an existing customer with the specified ID using the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to update.</param>
        /// <param name="customerDTO">The data transfer object containing updated customer details.</param>
        Task UpdateCustomerAsync(Guid id, CustomerDto customerDTO);

        /// <summary>
        /// Deletes the customer with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        Task DeleteCustomerAsync(Guid id);
    }
}
