using Dapper;
using Npgsql;
using System.Data;

namespace EmployeeTimeTrackingAPI
{
    public class DatabaseInitializer
    {
        private readonly string _defaultConnection;
        private readonly string _databaseName = "EmployeeDB";

        public DatabaseInitializer(IConfiguration configuration)
        {
            _defaultConnection = configuration.GetConnectionString("DefaultConnection");
        }

        public void Initialize()
        {
            var masterConnectionString = _defaultConnection.Replace($"Database={_databaseName};", "Database=postgres;");

            using (var connection = new NpgsqlConnection(masterConnectionString))
            {
                connection.Open();

                // Sprawdzenie, czy baza danych istnieje
                var databaseExists = connection.ExecuteScalar<bool>($@"
                    SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = '{_databaseName}');");

                // Jeśli nie istnieje, utwórz bazę danych
                if (!databaseExists)
                {
                    connection.Execute($"CREATE DATABASE \"{_databaseName}\";");
                }
            }

            // Połączenie z właściwą bazą
            using (var connection = new NpgsqlConnection(_defaultConnection))
            {
                connection.Open();

                // Tworzenie tabeli Employees
                var createEmployeesTable = @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        Id SERIAL PRIMARY KEY,
                        FirstName VARCHAR(100) NOT NULL,
                        LastName VARCHAR(100) NOT NULL,
                        Email VARCHAR(100) NOT NULL UNIQUE
                    );";

                // Tworzenie tabeli TimeEntries
                var createTimeEntriesTable = @"
                    CREATE TABLE IF NOT EXISTS TimeEntries (
                        Id SERIAL PRIMARY KEY,
                        EmployeeId INT REFERENCES Employees(Id) ON DELETE CASCADE,
                        Date DATE NOT NULL,
                        HoursWorked INT CHECK (HoursWorked >= 1 AND HoursWorked <= 24)
                    );";

                connection.Execute(createEmployeesTable);
                connection.Execute(createTimeEntriesTable);
            }
        }
    }
}
