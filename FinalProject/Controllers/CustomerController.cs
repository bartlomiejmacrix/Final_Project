using AutoMapper;
using FinalProject.Contracts;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerRepository customerRepository, ILogger<CustomerController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllAsync()
        {
            _logger.LogInformation("GET: Fetching all customers.");
            try
            {
                var customers = await _customerRepository.GetCustomersAsync();
                _logger.LogInformation("GET: Successfully retrieved {CustomerCount} customers.", customers.Count);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET: An error occurred while fetching customers.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateAsync([FromBody] CustomerDto createCustomerDTO)
        {
            if (createCustomerDTO == null)
            {
                _logger.LogWarning("POST: Received null customer DTO.");
                return BadRequest("Customer is null.");
            }

            _logger.LogInformation("POST: Creating a new customer.");
            try
            {
                var customer = await _customerRepository.CreateCustomerAsync(createCustomerDTO);
                _logger.LogInformation("POST: Successfully created customer with ID {CustomerId}.", customer.Id);
                return CreatedAtAction(nameof(GetAllAsync), new { id = customer.Id }, createCustomerDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST: An error occurred while creating a customer.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CustomerDto updatedCustomerDTO)
        {
            _logger.LogInformation("PUT: Updating customer with ID {CustomerId}.", id);
            try
            {
                await _customerRepository.UpdateCustomerAsync(id, updatedCustomerDTO);
                _logger.LogInformation("PUT: Successfully updated customer with ID {CustomerId}.", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "PUT: Concurrency error while updating customer with ID {CustomerId}.", id);
                return StatusCode(500, $"Concurrency error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PUT: An error occurred while updating customer with ID {CustomerId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _logger.LogInformation("DELETE: Deleting customer with ID {CustomerId}.", id);
            try
            {
                await _customerRepository.DeleteCustomerAsync(id);
                _logger.LogInformation("DELETE: Successfully deleted customer with ID {CustomerId}.", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex) 
            {
                _logger.LogError(ex, ex.Message, id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DELETE: An error occurred while deleting customer with ID {CustomerId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
