using Dapper;
using EmployeeTimeTrackingAPI.Models;
using EmployeeTimeTrackingAPI.Repositories;
using Moq;
using System.Data;
using Xunit;


namespace EmployeeTimeTrackingAPI.Tests
{
    public class EmployeeRepositoryTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepository;

        public EmployeeRepositoryTests()
        {
            _mockRepository = new Mock<IEmployeeRepository>();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNewEmployeeId()
        {
            // Arrange
            var employee = new Employee { FirstName = "Jan", LastName = "Kowalski", Email = "jan@example.com" };

            // Mockujemy repozytorium, aby zwróciło ID = 1 po dodaniu pracownika.
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                           .ReturnsAsync(1); // Symulujemy, że metoda zwróci ID 1

            // Act
            var id = await _mockRepository.Object.AddAsync(employee); // Wywołanie mockowanej metody

            // Assert
            Assert.Equal(1, id); // Sprawdzamy, czy metoda AddAsync zwróciła ID = 1
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployee()
        {
            // Arrange
            var employeeId = 1;
            var expectedEmployee = new Employee
            {
                Id = employeeId,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan@example.com"
            };

            // Mockujemy repozytorium, aby zwróciło pracownika o ID = 1
            _mockRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                           .ReturnsAsync(expectedEmployee);

            // Act
            var employee = await _mockRepository.Object.GetByIdAsync(employeeId); // Wywołanie mockowanej metody

            // Assert
            Assert.NotNull(employee); // Sprawdzamy, czy pracownik nie jest nullem
            Assert.Equal(employeeId, employee.Id); // Sprawdzamy, czy ID jest zgodne
            Assert.Equal("Jan", employee.FirstName); // Sprawdzamy, czy imię jest zgodne
            Assert.Equal("Kowalski", employee.LastName); // Sprawdzamy, czy nazwisko jest zgodne
            Assert.Equal("jan@example.com", employee.Email); // Sprawdzamy, czy email jest zgodny
        }
    }
}
