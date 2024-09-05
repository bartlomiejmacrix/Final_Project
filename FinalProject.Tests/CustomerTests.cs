using FakeItEasy;
using FinalProject.Contracts;
using FinalProject.Controllers;
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
        public async Task GetAll_ReturnsTwoCustomers_WhenTwoCustomersInDatabase()
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
        public async Task GetAll_ReturnsZeroCustomers_WhenNoCustomersInDatabase()
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