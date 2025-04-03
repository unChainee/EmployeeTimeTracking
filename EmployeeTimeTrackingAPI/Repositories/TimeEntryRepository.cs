using Dapper;
using EmployeeTimeTrackingAPI.Models;
using System.Data;

namespace EmployeeTimeTrackingAPI.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly IDbConnection _db;

        public TimeEntryRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TimeEntry>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _db.QueryAsync<TimeEntry>(
                "SELECT * FROM TimeEntries WHERE EmployeeId = @EmployeeId",
                new { EmployeeId = employeeId });
        }

        public async Task<TimeEntry?> GetByIdAsync(int employeeId, int timeEntryId)
        {
            return await _db.QueryFirstOrDefaultAsync<TimeEntry>(
                "SELECT * FROM TimeEntries WHERE EmployeeId = @EmployeeId AND Id = @Id",
                new { EmployeeId = employeeId, Id = timeEntryId });
        }

        public async Task<int> AddAsync(TimeEntry timeEntry)
        {
            var sql = "INSERT INTO TimeEntries (EmployeeId, Date, HoursWorked) VALUES (@EmployeeId, @Date, @HoursWorked) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, timeEntry);
        }

        public async Task<bool> UpdateAsync(TimeEntry timeEntry)
        {
            var sql = "UPDATE TimeEntries SET Date = @Date, HoursWorked = @HoursWorked WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, timeEntry) > 0;
        }

        public async Task<bool> DeleteAsync(int employeeId, int timeEntryId)
        {
            return await _db.ExecuteAsync(
                "DELETE FROM TimeEntries WHERE EmployeeId = @EmployeeId AND Id = @Id",
                new { EmployeeId = employeeId, Id = timeEntryId }) > 0;
        }
    }
}
