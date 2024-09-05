using AutoMapper;
using FinalProject.Contracts;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    /// <summary>
    /// Handles customer-related actions such as retrieving, creating, updating, and deleting customers.
    /// </summary>
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

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <response code="200">Returns the list of customers</response>
        /// <response code="500">If there was an internal server error</response>
        /// <response code="204">If no customers were found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllAsync()
        {
            _logger.LogInformation("GET: Fetching all customers.");
            try
            {
                var customers = await _customerRepository.GetCustomersAsync();
                if (customers.Count == 0)
                {
                    return NoContent();
                }
                _logger.LogInformation("GET: Successfully retrieved {CustomerCount} customers.", customers.Count);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET: An error occurred while fetching customers.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new customer in the database.
        /// </summary>
        /// <response code="201">Customer successfully created</response>
        /// <response code="400">If the provided customer data is null or invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return Created($"/api/customer/{customer.Id}", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST: An error occurred while creating a customer.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing customer with the provided data.
        /// </summary>
        /// <response code="204">Customer successfully updated</response>
        /// <response code="404">If the customer was not found</response>
        /// <response code="500">If there was an internal server error or concurrency issue</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <response code="204">Customer successfully deleted</response>
        /// <response code="404">If the customer was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
