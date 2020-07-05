using System;
using System.Data.SqlClient;
using System.IO;

namespace DbManagerConsoleJob
{
    static class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=sql-server-db;User Id=sa;Password=Password@1;";

            using (var connection = new SqlConnection(connectionString))
            {
                var toBeCreated = "CovidMicroServiceDB";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"CREATE DATABASE {toBeCreated}";
                command.ExecuteNonQuery();
            }


            Console.WriteLine("Hello World!");
        }

 
     


    }
}
