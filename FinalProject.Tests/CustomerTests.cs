using FakeItEasy;
using FinalProject.Contracts;
using FinalProject.Controllers;
using FinalProject.DTOs;
using FinalProject.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinalProject.Tests
{
    public class CustomerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerController _customerController;

        public CustomerTests()
        {
            _customerRepository = A.Fake<ICustomerRepository>();
            _logger = A.Fake<ILogger<CustomerController>>();
            _customerController = new CustomerController(_customerRepository, _logger);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsTwoCustomers_WhenTwoCustomersInDatabase()
        {
            // Arrange
            var customers = GetSampleCustomersList();
            A.CallTo(() => _customerRepository.GetCustomersAsync()).Returns(Task.FromResult(customers));

            // Act
            var result = await _customerController.GetAllAsync();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var actualCustomers = okResult?.Value as IEnumerable<Customer>;
            actualCustomers.Should().NotBeNull();
            actualCustomers.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsZeroCustomers_WhenNoCustomersInDatabase()
        {
            // Arrange
            var customers = new List<Customer>();
            A.CallTo(() => _customerRepository.GetCustomersAsync()).Returns(Task.FromResult(customers));

            // Act
            var result = await _customerController.GetAllAsync();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var actualCustomers = okResult?.Value as IEnumerable<Customer>;
            actualCustomers.Should().NotBeNull();
            actualCustomers.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateAsync_CreatesNewUser_WhenProvidedValidData()
        {
            // Arrange
            var customerToAdd = new CustomerDto
            {
                FirstName = "John",
                LastName = "Doe",
                StreetName = "Elm Street",
                HouseNumber = "123b",
                ApartmentNumber = "4B",
                PostalCode = "12-345",
                Town = "Springfield",
                PhoneNumber = "+1-555-1234",
                DateOfBirth = new DateTime(1985, 5, 15)
            };
            var customerFromResult = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                StreetName = "Elm Street",
                HouseNumber = "123b",
                ApartmentNumber = "4B",
                PostalCode = "12-345",
                Town = "Springfield",
                PhoneNumber = "+1-555-1234",
                DateOfBirth = new DateTime(1985, 5, 15)
            };
            A.CallTo(() => _customerRepository.CreateCustomerAsync(customerToAdd)).Returns(Task.FromResult(customerFromResult));
       
            // Act
            var result = await _customerController.CreateAsync(customerToAdd);

            // Assert
            var okResult = result.Result as CreatedAtActionResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeSameAs(customerToAdd);
        }

        [Fact]
        public async Task CreateAsync_ReturnsBadRequest_WhenProvidedCustomerIsNull()
        {
            // Arrange
            CustomerDto invalidCustomerDto = null;

            // Act
            var result = await _customerController.CreateAsync(invalidCustomerDto);

            // Assert
            var okResult = result.Result as BadRequestObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be("Customer is null.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenProvidedDataIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var oldCustomer = new CustomerDto
            {
                FirstName = "John",
                LastName = "Doe",
                StreetName = "Elm Street",
                HouseNumber = "123b",
                ApartmentNumber = "4B",
                PostalCode = "12-345",
                Town = "Springfield",
                PhoneNumber = "+1-555-1234",
                DateOfBirth = new DateTime(1985, 5, 15)
            };

            A.CallTo(() => _customerRepository.UpdateCustomerAsync(customerId, oldCustomer)).Returns(Task.CompletedTask);

            // Act
            var result = await _customerController.UpdateAsync(customerId, oldCustomer);

            // Assert
            var okResult = result as NoContentResult;
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenProvidedIdIsNotInDatabase()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var oldCustomer = new CustomerDto
            {
                FirstName = "John",
                LastName = "Doe",
                StreetName = "Elm Street",
                HouseNumber = "123b",
                ApartmentNumber = "4B",
                PostalCode = "12-345",
                Town = "Springfield",
                PhoneNumber = "+1-555-1234",
                DateOfBirth = new DateTime(1985, 5, 15)
            };

            A.CallTo(() => _customerRepository.UpdateCustomerAsync(customerId, oldCustomer))
                .Throws(new KeyNotFoundException($"Customer with ID {customerId} not found."));

            // Act
            var result = await _customerController.UpdateAsync(customerId, oldCustomer);

            // Assert
            var okResult = result as NotFoundObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be($"Customer with ID {customerId} not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenValidIdProvided()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            A.CallTo(() => _customerRepository.DeleteCustomerAsync(customerId)).Returns(Task.CompletedTask);

            // Act
            var result = _customerController.DeleteAsync(customerId);

            // Assert
            var okResult = result.Result as NoContentResult;
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_Throws_WhenProvidedIdIsNotInDatabase()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            A.CallTo(() => _customerRepository.DeleteCustomerAsync(customerId))
                .Throws(new KeyNotFoundException($"Customer with ID {customerId} not found."));

            // Act
            var result = await _customerController.DeleteAsync(customerId);

            // Assert
            var okResult = result as NotFoundObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be($"Customer with ID {customerId} not found.");
        }

        private List<Customer> GetSampleCustomersList()
        {
            return new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    StreetName = "Elm Street",
                    HouseNumber = "123b",
                    ApartmentNumber = "4B",
                    PostalCode = "12-345",
                    Town = "Springfield",
                    PhoneNumber = "+1-555-1234",
                    DateOfBirth = new DateTime(1985, 5, 15)
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    StreetName = "Oak Avenue",
                    HouseNumber = "456",
                    ApartmentNumber = "2A",
                    PostalCode = "67-890",
                    Town = "Shelbyville",
                    PhoneNumber = "+1-555-5678",
                    DateOfBirth = new DateTime(1990, 8, 25)
                }
            };
        }

    }
}