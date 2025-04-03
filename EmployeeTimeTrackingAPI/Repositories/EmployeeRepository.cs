using Dapper;
using EmployeeTimeTrackingAPI.Models;
using Npgsql;
using System.Data;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnection _db;

    public EmployeeRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _db.QueryAsync<Employee>("SELECT * FROM Employees");
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _db.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employees WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> AddAsync(Employee employee)
    {
        var sql = "INSERT INTO Employees (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email) RETURNING Id";
        return await _db.ExecuteScalarAsync<int>(sql, employee);
    }

    public async Task<bool> UpdateAsync(Employee employee)
    {
        var sql = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE Id = @Id";
        return await _db.ExecuteAsync(sql, employee) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _db.ExecuteAsync("DELETE FROM Employees WHERE Id = @Id", new { Id = id }) > 0;
    }
}
