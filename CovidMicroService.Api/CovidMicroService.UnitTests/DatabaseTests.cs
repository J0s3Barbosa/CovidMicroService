using System;
using System.Data.SqlClient;
using Xunit;

namespace CovidMicroService.UnitTests
{
    public class DatabaseTests
    {
        readonly string connectionString = "Server=localhost;User Id=sa;Password=Password@1;";

        [Fact(DisplayName = "Cenario - create databse")]
        [Trait("Category", "Success")]
        public void Create_database()
        {
            bool actualResult = false;
            var toBeCreated = "CovidMicroServiceDB";
            var result = CheckDatabaseExists(connectionString, toBeCreated);
            if (!result)
            {
                actualResult = CreateDatabase(connectionString, toBeCreated);
            }

            Assert.True(actualResult);
        }

        [Fact(DisplayName = "Cenario - create databse")]
        [Trait("Category", "Fail")]
        public void Create_database_Failure()
        {
            bool actualResult = false;
            var toBeCreated = "CovidMicroServiceDB";
            var result = CheckDatabaseExists(connectionString, toBeCreated);
            if (!result)
            {
                actualResult = CreateDatabase(connectionString, toBeCreated);
            }

            Assert.False(actualResult);
        }

        private bool CreateDatabase(string connectionString, string toBeCreated)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"CREATE DATABASE {toBeCreated}";
                return command.ExecuteNonQuery() > 0;
            }
        }

        [Fact(DisplayName = "Cenario - check databse exists")]
        [Trait("Category", "Fail")]
        public void Check_If_database_Exists_Failure()
        {
            var databaseName = "CovidMicroServiceDB2";

            var result = CheckDatabaseExists(connectionString, databaseName);

            Assert.False(result);
        }
        [Fact(DisplayName = "Cenario - check databse exists")]
        [Trait("Category", "Success")]
        public void Check_If_database_Exists_Success()
        {
            var databaseName = "CovidMicroServiceDB";

            var result = CheckDatabaseExists(connectionString, databaseName);

            Assert.True(result);
        }
        private static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            string sqlCreateDBQuery;
            sqlCreateDBQuery = $"SELECT db_id('{databaseName}')";

            using var connection = new SqlConnection(connectionString);
            using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, connection))
            {
                connection.Open();
                return (sqlCmd.ExecuteScalar() != DBNull.Value);
            }

        }




    }
}
