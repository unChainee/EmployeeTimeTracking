using Dapper;
using EmployeeTimeTrackingAPI.Models;
using Npgsql;
using System.Data;
using Xunit;

namespace EmployeeTimeTrackingAPI.Tests
{
    public class EmployeeRepositoryIntegrationTests
    {
        private readonly string _connectionString;

        public EmployeeRepositoryIntegrationTests()
        {
            // Połączenie do testowej bazy danych PostgreSQL (możesz użyć kontenera Docker)
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=EmployeeDB";
        }

        private async Task<IDbConnection> GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        [Fact]
        public async Task AddEmployee_ShouldReturnNewEmployeeId()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan@example.com"
            };

            // Utworzenie repozytorium
            using (var connection = await GetConnection())
            {
                var repository = new EmployeeRepository(connection);

                // Act
                var employeeId = await repository.AddAsync(employee);

                // Assert
                Assert.True(employeeId > 0, "Employee ID should be greater than 0");

                // Clean up
                await connection.ExecuteAsync("DELETE FROM Employees WHERE Id = @Id", new { Id = employeeId });
            }
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnCorrectEmployee()
        {
            // Arrange - dodanie pracownika do bazy
            var employee = new Employee
            {
                FirstName = "Anna",
                LastName = "Nowak",
                Email = "anna@example.com"
            };

            using (var connection = await GetConnection())
            {
                var repository = new EmployeeRepository(connection);

                // Dodanie pracownika do bazy
                var employeeId = await repository.AddAsync(employee);

                // Act - pobranie pracownika z bazy
                var retrievedEmployee = await repository.GetByIdAsync(employeeId);

                // Assert
                Assert.NotNull(retrievedEmployee);
                Assert.Equal(employeeId, retrievedEmployee.Id);
                Assert.Equal(employee.FirstName, retrievedEmployee.FirstName);
                Assert.Equal(employee.LastName, retrievedEmployee.LastName);
                Assert.Equal(employee.Email, retrievedEmployee.Email);

                // Clean up
                await connection.ExecuteAsync("DELETE FROM Employees WHERE Id = @Id", new { Id = employeeId });
            }
        }
    }
}
