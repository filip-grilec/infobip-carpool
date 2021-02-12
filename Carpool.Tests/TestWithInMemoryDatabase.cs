using System;
using System.Linq;
using Carpool;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace OkrApp.Tests
{
    public abstract class TestWithInMemoryDatabase : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly CarpoolContext DbContext;

        protected TestWithInMemoryDatabase()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<CarpoolContext>()
                .UseSqlite(_connection)
                .Options;
            DbContext = new CarpoolContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        protected void ClearDatabaseForConcurrencyReasons()
        {
            DbContext.RemoveRange(DbContext.Cars);
            DbContext.RemoveRange(DbContext.Locations);
            DbContext.RemoveRange(DbContext.Employees);
            DbContext.RemoveRange(DbContext.TravelPlanEmployees);
            DbContext.RemoveRange(DbContext.TravelPlans);
            DbContext.SaveChanges();
        }

        /// <summary>
        /// Stops tracking entities, otherwise include tests always pass
        /// </summary>
        protected void PrepareForIncludeTest()
        {
            DbContext.GetDependencies().StateManager.ResetState();
        }
    }

}