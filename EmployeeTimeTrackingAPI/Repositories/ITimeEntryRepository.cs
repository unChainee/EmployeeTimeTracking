using EmployeeTimeTrackingAPI.Models;

namespace EmployeeTimeTrackingAPI.Repositories
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntry>> GetByEmployeeIdAsync(int employeeId);
        Task<TimeEntry?> GetByIdAsync(int employeeId, int timeEntryId);
        Task<int> AddAsync(TimeEntry timeEntry);
        Task<bool> UpdateAsync(TimeEntry timeEntry);
        Task<bool> DeleteAsync(int employeeId, int timeEntryId);
    }
}
