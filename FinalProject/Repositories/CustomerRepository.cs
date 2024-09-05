using AutoMapper;
using FinalProject.Contracts;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class CustomerRepository(AppDbContext context, IMapper mapper) : ICustomerRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Customer> CreateCustomerAsync(CustomerDto customerDTO)
        {
            var customer = _mapper.Map<Customer>(customerDTO);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id) ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Guid id, CustomerDto customerDTO)
        {
            if (!await _context.Customers.AnyAsync(c => c.Id == id))
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            var updatedCustomer = _mapper.Map<Customer>(customerDTO);
            updatedCustomer.Id = id;
            _context.Customers.Update(updatedCustomer);
            await _context.SaveChangesAsync();
        }
    }
}
