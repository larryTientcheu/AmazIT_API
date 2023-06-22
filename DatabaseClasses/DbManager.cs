using SampleRESTAPI.Models;
using System.Data;
using System.Data.SQLite;

namespace SampleRESTAPI.DatabaseClasses
{
    public abstract class DbManager
    {
        protected readonly string connectionString;

        public DbManager(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
